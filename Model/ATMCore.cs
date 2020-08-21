﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ATM
{
    public class ATMCore : INotifyPropertyChanged
    {
        private Dictionary<int, int> atm_storage = new Dictionary<int, int>() { {10, 0}, { 50, 0}, { 100, 0 },
                        { 200, 0 }, { 500, 0 }, { 1000, 0 }, { 2000, 0 }, { 5000, 0 } };
        private const int minNominal = 10;
        private int capacity = 0;
        private int amountOfBanknotes = 0;
        private int countOfBanknotes = 0;
        private int remainingSpace = 0;

        public event PropertyChangedEventHandler PropertyChanged;

        public int Capacity
        {
            get { return capacity; }
            set
            {
                capacity = value;
                OnPropertyChanged("Capacity");
            }
        }

        public int AmountOfBanknotes
        {
            get { return amountOfBanknotes; }
            set
            {
                amountOfBanknotes = value;
                OnPropertyChanged("AmountOfBanknotes");
            }
        }

        public int GetBacknotesCount(int nominal)
        {
            if (Storage.ContainsKey(nominal))
                return Storage[nominal];
            else
                return 0;
        }

        public bool SetBacknotesCount(int nominal, int count)
        {
            if (Storage.ContainsKey(nominal))
            {
                Storage[nominal] = count;
                return true;
            }
            else
                return false;
        }
        public int CountOfBanknotes
        {
            get { return countOfBanknotes; }
            set
            {
                countOfBanknotes = value;
                OnPropertyChanged("CountOfBanknotes");
            }
        }

        public Dictionary<int, int> Storage { get => atm_storage; set => atm_storage = value; }
        public int RemainingSpace { get => remainingSpace;
            set
            {
                remainingSpace = value;
                OnPropertyChanged("RemainingSpace");
            }
        }

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        
        ATMCore(Dictionary<int, int> atm_storage)
        {
            this.Storage = atm_storage;
        }

        public ATMCore(int capacity)
        {
            this.capacity = capacity;
        }

        public void AddMoney(int value, int count = 1)
        {
            if (Storage.Count >= capacity)
                throw new Exception("Банкомат полон!");

            if (count > 0)
            {
                if (Storage.ContainsKey(value))
                    Storage[value] += count;
                else
                    Storage.Add(value, count);

                CountOfBanknotes += count;
                AmountOfBanknotes += count * value;
                RemainingSpace = capacity - countOfBanknotes;
            }
        }

        public bool PickUpMoney(int value, int count = 1)
        {
            if(count <= 0)
                return false;
            if(Storage.Count == 0)
                throw new Exception("Банкомат пуст!");
            if (count > 0)
            {
                if (Storage.ContainsKey(value))
                {
                    if (Storage[value] >= count)
                    {
                        Storage[value] -= count;
                        CountOfBanknotes -= count;
                        AmountOfBanknotes -= count * value;
                        RemainingSpace = capacity - countOfBanknotes;
                        return true;
                    }
                }
            }
 
            throw new Exception("Банкомат не может выдать данную сумму!");
        }

        public void PickUpMoney(int amountToIssue)
        {
            int[,] decisionMatrix;
            Dictionary<int,int> solution;

            ProcessingPickUp(amountToIssue, out decisionMatrix);
            SearchSolution(decisionMatrix, amountToIssue, out solution);
            IssuanceProcess(solution);

        }

        public void PickUpMoney(int amountToIssue, out Dictionary<int, int> solution)
        {
            int[,] decisionMatrix;
            if (amountToIssue % minNominal > 0)
                throw new Exception("Введенная сумма для выдачи должна быть кратна: " + minNominal + "!");
            ProcessingPickUp(amountToIssue, out decisionMatrix);
            SearchSolution(decisionMatrix, amountToIssue, out solution);
            IssuanceProcess(solution);

        }
        public void ProcessingPickUp(int amountToIssue, out int[,] decisionMatrix)
        {
            int max_value = Storage.Values.Max();
            int size = (int)(amountToIssue / minNominal);
            if (size <= 0)
                throw new Exception("Запращиваемая сумма не может быть отрицательной или равна 0!");
            decisionMatrix = new int[Storage.Keys.Count, size];
            List<int> denominations = Storage.Keys.ToList<int>();
            int local_sum = 0;

            for(int i = 0; i < denominations.Count;i++)
            {
                local_sum = minNominal;
                for (int j = 0; j < size; j++)
                {
                    if (local_sum <= amountToIssue && local_sum >= denominations[i])
                    {
                        int quantityForIssue = (int)(local_sum / denominations[i]);
                        

                        if (i - 1 >= 0)
                        {
                            if (quantityForIssue > atm_storage[denominations[i]])
                                quantityForIssue = atm_storage[denominations[i]];
                            
                            int idx = ((local_sum - denominations[i] * quantityForIssue) / minNominal) - 1;
                            int quantityForIssue2 =  (idx >= 0 && decisionMatrix[i - 1, idx] > 0) ? decisionMatrix[i - 1, idx] + quantityForIssue : 0;
                            int quantityForIssue3 = decisionMatrix[i - 1, j];
                            if (local_sum - denominations[i] * quantityForIssue != 0)
                                quantityForIssue = 0;
                            decisionMatrix[i, j] = MinNoZero(quantityForIssue, quantityForIssue2, quantityForIssue3);
                        }
                        else
                        {
                            if (quantityForIssue > atm_storage[denominations[i]])
                                quantityForIssue = 0;
                            decisionMatrix[i, j] = quantityForIssue;
                        }
                        

                    }
                    else
                    {
                        if (i == 0)
                            decisionMatrix[i, j] = 0;
                        else
                            decisionMatrix[i, j] = decisionMatrix[i - 1, j];
                    }

                    local_sum += minNominal;
                }            
            }  
        }

        public int MinNoZero(params int[] values)
        {
            int minValue = int.MaxValue;
            for (int i = 0; i < values.Length; i++)
                if (minValue > values[i] && values[i] != 0)
                    minValue = values[i];

            return (minValue == int.MaxValue) ? 0 : minValue;
        }

        public bool SearchSolution(int[,] decisionMatrix, int amountToIssue,out Dictionary<int,int> solution)
        {
            int idx = 0;
            int k = Storage.Keys.Count;
            List<int> passedDenominationsIdx = new List<int>();
            List<int> denominations = Storage.Keys.ToList<int>();
            solution = new Dictionary<int, int>();
            int i_size = decisionMatrix.GetLength(0);
            while (amountToIssue > 0)
            {
                if (passedDenominationsIdx.Count == k)
                    return false;
                if (amountToIssue < minNominal)
                    return false;

                idx = (int)(amountToIssue / minNominal) - 1;

                int resultIdx = 0;

                for (int i = i_size - 1; i > 0; i--)
                {
                    if (passedDenominationsIdx.Contains(i))
                        continue;
                    Debug.WriteLine(decisionMatrix[i, idx]);
                    if (decisionMatrix[i, idx] == decisionMatrix[i - 1, idx])
                        continue;
                    else
                    {
                        
                        Debug.WriteLine(decisionMatrix[i - 1, idx]);
                        resultIdx = i;
                        break;
                    }
                }
                if (decisionMatrix[resultIdx, idx] == 0)
                    return false;
                else
                {
                    int maxCount = (int) (amountToIssue / denominations[resultIdx]);
                    if(maxCount > decisionMatrix[resultIdx, idx])
                        maxCount = decisionMatrix[resultIdx, idx];
                    if (maxCount > Storage[denominations[resultIdx]])
                        maxCount = Storage[denominations[resultIdx]];


                    amountToIssue -= maxCount * denominations[resultIdx];              
                    AddSolution(ref solution, denominations[resultIdx], maxCount);

                }

                passedDenominationsIdx.Add(resultIdx);
            }

            return true;
        }

        private void AddSolution(ref Dictionary<int,int> solution, int new_banknote, int count)
        {
            if (solution.ContainsKey(new_banknote))
                solution[new_banknote]+= count;
            else
                solution.Add(new_banknote, count);
        }

        public void IssuanceProcess(Dictionary<int, int> solution)
        {
            foreach (KeyValuePair<int,int> banknotes in solution)
                PickUpMoney(banknotes.Key, banknotes.Value);
            
        }
    }
}
