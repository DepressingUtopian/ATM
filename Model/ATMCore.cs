using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ATM
{
    public class ATMCore : INotifyPropertyChanged
    {
        private Dictionary<int, int> atm_storage = new Dictionary<int, int>();
        private const int minNominal = 10;
        private int capacity = 0;
        private int amountOfBanknotes = 0;
        private int countOfBanknotes = 0;

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
            if (atm_storage.ContainsKey(nominal))
                return atm_storage[nominal];
            else
                return 0;
        }

        public bool SetBacknotesCount(int nominal, int count)
        {
            if (atm_storage.ContainsKey(nominal))
            {
                atm_storage[nominal] = count;
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
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        
        ATMCore(Dictionary<int, int> atm_storage)
        {
            this.atm_storage = atm_storage;
        }

        public ATMCore(int capacity)
        {
            this.capacity = capacity;
        }

        public void AddMoney(int value, int count = 1)
        {
            if (atm_storage.Count >= capacity)
                throw new Exception("Банкомат полон!");

            if (atm_storage.ContainsKey(value))
                atm_storage[value] += count;
            else
                atm_storage.Add(value,count);

            countOfBanknotes += count;
            amountOfBanknotes += count * value;
        }

        public bool PickUpMoney(int value, int count = 1)
        {
            if(count <= 0)
                return false;
            if(atm_storage.Count == 0)
                throw new Exception("Банкомат пуст!");

            if (atm_storage.ContainsKey(value))
            {
                if (atm_storage[value] >= count)
                {
                    atm_storage[value] -= count;
                    countOfBanknotes -= count;
                    amountOfBanknotes -= count * value;
                    return true;
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

        public void ProcessingPickUp(int amountToIssue, out int[,] decisionMatrix)
        {
            int max_value = atm_storage.Values.Max();
            int size = (int)(amountToIssue / minNominal);
            if (size <= 0)
                throw new Exception("Запращиваемая сумма не может быть отрицательной!");
            decisionMatrix = new int[atm_storage.Keys.Count, size];
            List<int> denominations = atm_storage.Keys.ToList<int>();
            int local_sum = 0;

            for(int i = 0; i < denominations.Count;i++)
            {
                local_sum = minNominal;
                for (int j = 0; j < size; j++)
                {
                    if (local_sum <= amountToIssue && local_sum >= denominations[i])
                    {
                        int quantityForIssue = (int)(local_sum / denominations[i]);
                        if (quantityForIssue > 0 && atm_storage[denominations[i]] >= quantityForIssue)
                            decisionMatrix[i, j] = quantityForIssue;
                        else
                            decisionMatrix[i, j] = -1;
                    }
                    else
                    {
                        if (j - 1 > 0)
                            decisionMatrix[i, j] = decisionMatrix[i, j - 1];
                        else
                            decisionMatrix[i, j] = -1;
                    }

                    local_sum += minNominal;
                }            
            }  
        }

        public bool SearchSolution(int[,] decisionMatrix, int amountToIssue,out Dictionary<int,int> solution)
        {
            int idx = 0;
            int k = atm_storage.Keys.Count;
            List<int> passedDenominationsIdx = new List<int>();
            List<int> denominations = atm_storage.Keys.ToList<int>();
            solution = new Dictionary<int, int>();
            while (amountToIssue != 0)
            {
                if (passedDenominationsIdx.Count == k)
                    return false;
                if (amountToIssue < minNominal)
                    return false;

                idx = (int)(amountToIssue / minNominal) - 1;
                int minCountIdx = 0;
                int minElem = Int32.MaxValue;
                for (int i = 1; i < k; i++)
                {
                    if (passedDenominationsIdx.Contains(minCountIdx))
                    {
                        minCountIdx++;
                        continue;
                    }
                    if (minElem > decisionMatrix[i, idx] && decisionMatrix[i, idx] != -1)
                    {
                        minElem = decisionMatrix[i, idx];
                        minCountIdx = i;
                    }
                }
                if (decisionMatrix[minCountIdx, idx] == -1)
                    return false;
                else
                {
                    amountToIssue -= decisionMatrix[minCountIdx, idx] * denominations[minCountIdx];              
                    AddSolution(ref solution, denominations[minCountIdx], decisionMatrix[minCountIdx, idx]);

                }

                passedDenominationsIdx.Add(minCountIdx);
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
