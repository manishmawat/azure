using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleToDoTableApi.Repositories
{
    public class ToDoRepository
    {
        private CloudTable todoTable = null;
        public ToDoRepository()
        {
            var storageAccount = CloudStorageAccount.Parse("");

            var tableClient = storageAccount.CreateCloudTableClient();
            todoTable = tableClient.GetTableReference("Todo");

            todoTable.CreateIfNotExists();
        }
        
        public IEnumerable<TodoEntity> All()
        {
            var query = new TableQuery<TodoEntity>().Where(TableQuery.GenerateFilterConditionForBool(nameof(TodoEntity.Completed), 
                QueryComparisons.Equal, 
                false));
            var entities = todoTable.ExecuteQuery(query);
            return entities;
        }

        public void Create(TodoEntity entity)
        {
            var operation = TableOperation.InsertOrReplace(entity);
            todoTable.Execute(operation);
        }

        public void Delete(TodoEntity entity)
        {
            var operation = TableOperation.Delete(entity);
            todoTable.Execute(operation);
        }

        public TodoEntity Get(string partitionKey, string rowKey)
        {
            var operation = TableOperation.Retrieve<TodoEntity>(partitionKey, rowKey);
            var result = todoTable.Execute(operation);

            return result.Result as TodoEntity;

        }
    }

    public class TodoEntity:TableEntity
    {
        public string Content { get; set; }
        public bool Completed { get; set; }

        public string Due { get; set; }
    }
}
