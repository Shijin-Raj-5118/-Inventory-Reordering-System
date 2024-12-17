using System;
using System.Collections.Generic;
using System.Linq;
public class InventoryItem
{
    public string ItemId { get; set; }
    public int CurrentStock { get; set; }
    public int ForecastedDemand { get; set; }
    public decimal ReorderCostPerUnit { get; set; }
    public int BatchSize { get; set; }
}
public class ReorderPlan
{
    public string ItemId { get; set; }
    public int UnitsToOrder { get; set; }
    public decimal TotalCost { get; set; }
}
public class InventoryReorder
{
    public static List<ReorderPlan> GenerateReorderingPlan(List<InventoryItem> items)
    {
        var itemsToReorder = items
        .Select(item => new
        {
            item.ItemId,
            Deficit = item.ForecastedDemand - item.CurrentStock,
            item.ReorderCostPerUnit,
            item.BatchSize
        })
        .Where(x => x.Deficit > 0)
        .OrderBy(x => x.ReorderCostPerUnit / x.BatchSize) // Sort by cost-efficiency
        .ToList();
        var reorderingPlan = new List<ReorderPlan>();
        foreach (var item in itemsToReorder)
        {
            int unitsToOrder = (int)Math.Ceiling((decimal)item.Deficit / item.BatchSize) *
           item.BatchSize;
            decimal totalCost = unitsToOrder * item.ReorderCostPerUnit;
            reorderingPlan.Add(new ReorderPlan
            {
                ItemId = item.ItemId,
                UnitsToOrder = unitsToOrder,
                TotalCost = totalCost
            });
        }
        return reorderingPlan;
    }
    public static void Main(string[] args)
    {
        var items = new List<InventoryItem>
 {
 new InventoryItem { ItemId = "A1", CurrentStock = 50, ForecastedDemand = 100,
ReorderCostPerUnit = 2.5M, BatchSize = 20 },
 new InventoryItem { ItemId = "B2", CurrentStock = 200, ForecastedDemand = 150,
ReorderCostPerUnit = 1.5M, BatchSize = 50 },
 new InventoryItem { ItemId = "C3", CurrentStock = 30, ForecastedDemand = 120,
ReorderCostPerUnit = 3.0M, BatchSize = 10 }
 };
        var plan = GenerateReorderingPlan(items);
        Console.WriteLine("Reordering Plan:");
        foreach (var entry in plan)
        {
            Console.WriteLine($"Item: {entry.ItemId}, Units to Order: {entry.UnitsToOrder}, Total Cost:{ entry.TotalCost:C}");
        }
    }
}
