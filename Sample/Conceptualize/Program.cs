using Conceptualize;
using Dumpify;

var orders = SerializeList.CreateOrders();
var json = SerializeList.SerializeOrders(orders);
json.DumpConsole();
