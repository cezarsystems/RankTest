using System;
using System.Collections.Generic;
using System.Linq;

namespace Testes
{
    class Program
    {
        static void Main(string[] args)
        {
            List<AfastamentoTemporario> mockData = new List<AfastamentoTemporario>
            {
                new AfastamentoTemporario { OrganizationName = "AAA2B", OrkPk = 5, StartDate = DateTime.Now.AddDays(-19).Date, EndDate = DateTime.Now.AddDays(-13).Date },
                new AfastamentoTemporario { OrganizationName = "AAA2A", OrkPk = 3, StartDate = DateTime.Now.AddDays(-20).Date, EndDate = DateTime.Now.AddDays(-15).Date },
                new AfastamentoTemporario { OrganizationName = "Cushman TC", OrkPk = 34, StartDate = DateTime.Now.AddDays(-60).Date, EndDate = DateTime.Now.AddDays(-13).Date, AreaId = 55 },
                new AfastamentoTemporario { OrganizationName = "AAA", OrkPk = 73, StartDate = DateTime.Now.AddDays(-19).Date, EndDate = DateTime.Now.AddDays(-13).Date },
                new AfastamentoTemporario { OrganizationName = "Engebanco", OrkPk = 35, StartDate = DateTime.Now.AddDays(-25).Date, EndDate = DateTime.Now.AddDays(-13).Date },
                new AfastamentoTemporario { OrganizationName = "Engebanc", OrkPk = 38, StartDate = DateTime.Now.AddDays(-24).Date, EndDate = DateTime.Now.AddDays(-13).Date },
                new AfastamentoTemporario { OrganizationName = "Cushman", OrkPk = 3, StartDate = DateTime.Now.AddDays(-60).Date, EndDate = DateTime.Now.AddDays(-13).Date, AreaId = 55 },
                new AfastamentoTemporario { OrganizationName = "LVC", OrkPk = 77, StartDate = DateTime.Now.AddDays(-60).Date, EndDate = DateTime.Now.AddDays(-13).Date },
                new AfastamentoTemporario { OrganizationName = "Engemax", OrkPk = 61, StartDate = DateTime.Now.AddDays(-256).Date, EndDate = DateTime.Now.AddDays(-17).Date },
                new AfastamentoTemporario { OrganizationName = "Engemax2", OrkPk = 69, StartDate = DateTime.Now.AddDays(-200).Date, EndDate = DateTime.Now.AddDays(-16).Date },
                new AfastamentoTemporario { OrganizationName = "Engebanc", OrkPk = 38, StartDate = DateTime.Now.AddDays(-200).Date, EndDate = DateTime.Now.AddDays(-156).Date, AreaId = 55 },                
                new AfastamentoTemporario { OrganizationName = "LVC", OrkPk = 77, StartDate = DateTime.Now.AddDays(-15).Date, EndDate = DateTime.Now.AddDays(-7).Date, AreaId = 89 }
            };

            #region Temporary General Removal - Begin

            Console.WriteLine("Temporary General Removal:\n");

            // Nullable Verification
            if (mockData?.Count == 0)
                return;

            // List of except items
            List<AfastamentoTemporario> listToRemove = new List<AfastamentoTemporario>();

            // Temporary General Removal list
            List<AfastamentoTemporario> temporaryGeneralRemovalList = mockData
                .Where(x => x.AreaId is null)
                .ToList();

            // Nullable Verification
            if (temporaryGeneralRemovalList?.Count == 0)
                return;

            // All Organizations that returned on the same date within the Temporary General Removal list
            List<AfastamentoTemporario> sameEndDateByOrgAtTemporaryGeneralRemovalList = temporaryGeneralRemovalList
                .GroupBy(x => x.EndDate.Date)
                .Where(y => y.Count() > 1)
                .SelectMany(z => z)
                .ToList();

            // Best ranked item
            AfastamentoTemporario bestItemFoundInRanking =
                ProcessRankByIntervalDaysAndAlphabeticalOrder(temporaryGeneralRemovalList, sameEndDateByOrgAtTemporaryGeneralRemovalList);

            // Adds the object to be ignored for the rest of the process
            listToRemove.Add(bestItemFoundInRanking);

            // Print partial item
            PrintToTest(bestItemFoundInRanking);

            // Nullable Verification
            if (sameEndDateByOrgAtTemporaryGeneralRemovalList?.Count > 0)
            {
                // First, process the overall list of Temporary Removal with the same EndDate
                sameEndDateByOrgAtTemporaryGeneralRemovalList.Except(listToRemove).OrderBy(x => x.OrganizationName)
                    .OrderByDescending(x => (x.StartDate - x.EndDate).Days).ToList().ForEach(item =>
                    {
                        // Print partial item
                        PrintToTest(item);

                        // Adds the object to be ignored for the rest of the process
                        listToRemove.Add(item);
                    });

                // Second, process the general list of Temporary Removal
                temporaryGeneralRemovalList.Except(sameEndDateByOrgAtTemporaryGeneralRemovalList).Except(listToRemove).OrderBy(x => x.OrganizationName)
                    .OrderByDescending(x => (x.StartDate - x.EndDate).Days).ToList().ForEach(item =>
                    {
                        // Print partial item
                        PrintToTest(item);                        
                    });               
            }
            // Process the general list of Temporary Removal
            else
            {
                temporaryGeneralRemovalList.Except(listToRemove).OrderBy(x => x.OrganizationName)
                   .OrderByDescending(x => (x.StartDate - x.EndDate).Days).ToList().ForEach(item =>
                   {
                       // Print partial item
                       PrintToTest(item);
                   });
            }

            // Cleaning List to Remove
            listToRemove.Clear();

            #endregion Temporary General Removal - End

            #region Temporary Removal by Polo - Begin

            Console.WriteLine("\nTemporary Removal by Polo:\n");

            // Temporary Removal by Polo list
            List<AfastamentoTemporario> temporaryRemovalByPoloList = mockData
                .Where(x => x.AreaId != null)
                .ToList();

            // Nullable Verification
            if (temporaryRemovalByPoloList?.Count == 0)
                return;

            // All Organizations that returned on the same date and serve the same area within the Temporary Removal By Polo list
            List<AfastamentoTemporario> sameEndDateAndAreaOrgsByTemporaryRemovalByPoloList = temporaryRemovalByPoloList
                .GroupBy(x => x.EndDate.Date)
                .Where(y => y.Count() > 1)
                .SelectMany(z => z)
                .GroupBy(x => x.AreaId)
                .Where(y => y.Count() > 1)
                .SelectMany(y => y)
                .ToList();

            // Best ranked item
            bestItemFoundInRanking = ProcessRankByIntervalDaysAndAlphabeticalOrder(temporaryRemovalByPoloList, sameEndDateAndAreaOrgsByTemporaryRemovalByPoloList);

            // Adds the object to be ignored for the rest of the process
            listToRemove.Add(bestItemFoundInRanking);

            // Print partial item
            PrintToTest(bestItemFoundInRanking);

            // Nullable Verification
            if (sameEndDateAndAreaOrgsByTemporaryRemovalByPoloList?.Count > 0)
            {
                // First, process the general list of Poles
                temporaryRemovalByPoloList.Except(sameEndDateAndAreaOrgsByTemporaryRemovalByPoloList).Except(listToRemove).OrderBy(x => x.OrganizationName)
                    .OrderByDescending(x => (x.StartDate - x.EndDate).Days).ToList().ForEach(item =>
                    {
                        // Print partial item
                        PrintToTest(item);

                        // Adds the object to be ignored for the rest of the process
                        listToRemove.Add(item);
                    });

                // Second, process the overall list of Poles with the same EndDate and AreaId
                sameEndDateAndAreaOrgsByTemporaryRemovalByPoloList.Except(listToRemove).OrderBy(x => x.OrganizationName)
                    .OrderByDescending(x => (x.StartDate - x.EndDate).Days).ToList().ForEach(item =>
                    {
                        // Print partial item
                        PrintToTest(item);
                    });
            }
            // Process the general list of Poles
            else
            {
                temporaryRemovalByPoloList.Except(listToRemove).OrderBy(x => x.OrganizationName)
                   .OrderByDescending(x => (x.StartDate - x.EndDate).Days).ToList().ForEach(item =>
                   {
                        // Print partial item
                        PrintToTest(item);
                   });
            }

            #endregion Temporary Removal by Polo - End

            Console.ReadKey();
        }

        static AfastamentoTemporario ProcessRankByIntervalDaysAndAlphabeticalOrder(List<AfastamentoTemporario> mainList, List<AfastamentoTemporario> listToProcess)
        {
            // Smaller interval in the mainList
            AfastamentoTemporario smallerIntervalFromMainList = mainList
                .OrderBy(x => x.OrganizationName)
                .OrderBy(x => (x.EndDate - x.StartDate).Days)
                .FirstOrDefault();

            // Nullable Verification
            if (listToProcess?.Count == 0)
                return smallerIntervalFromMainList;

            // Smaller interval in the listToProcess
            AfastamentoTemporario smallerIntervalFromToProcessList = listToProcess
                .OrderBy(x => x.OrganizationName)
                .OrderBy(x => (x.EndDate - x.StartDate).Days)
                .FirstOrDefault();            

            // Equality object
            if (!smallerIntervalFromMainList.Equals(smallerIntervalFromToProcessList))
            {
                // Equal number of days removed temporality
                if ((smallerIntervalFromMainList.EndDate - smallerIntervalFromMainList.StartDate).Days ==
                    (smallerIntervalFromToProcessList.EndDate - smallerIntervalFromToProcessList.StartDate).Days)
                {
                    // Alphabetical order
                    if (string.Compare(smallerIntervalFromMainList.OrganizationName, smallerIntervalFromToProcessList.OrganizationName) == -1)
                        return smallerIntervalFromMainList;
                    else
                        return smallerIntervalFromToProcessList;
                }
                else if ((smallerIntervalFromMainList.EndDate - smallerIntervalFromMainList.StartDate).Days >
                         (smallerIntervalFromToProcessList.EndDate - smallerIntervalFromToProcessList.StartDate).Days)
                    return smallerIntervalFromToProcessList;
                else
                    return smallerIntervalFromMainList;
            }
            else
                return smallerIntervalFromMainList;
        }

        static void PrintToTest(AfastamentoTemporario itemToPrint)
        {
            Console.WriteLine(
                "Nome Org: " + itemToPrint.OrganizationName +
                " OrgPk: " + itemToPrint.OrkPk +
                " start date: " + itemToPrint.StartDate +
                " end date: " + itemToPrint.EndDate +
                " dias afastados: " + (itemToPrint.StartDate - itemToPrint.EndDate).Days +
                " area id: " + (itemToPrint.AreaId ?? 0)
            );
        }
    }

    class AfastamentoTemporario
    {
        public string OrganizationName { get; set; }
        public int OrkPk { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int? AreaId { get; set; }
    }
}