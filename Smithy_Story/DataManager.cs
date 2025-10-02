using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smithy_Story
{
    public class DataManager<T> where T : IItem
    {
        private Dictionary<int, T> itemsById = new Dictionary<int, T>();
        private Dictionary<string, int> nameToId = new Dictionary<string, int>();

        public void AddItem(T item)
        {
            if (!itemsById.ContainsKey((item as IItem).ID))
            {
                itemsById[item.ID] = item;
                nameToId[item.Name] = item.ID;
            }
        }

        public T GetById(int id)
        {
            if (itemsById.ContainsKey(id))
                return itemsById[id];

            throw new ArgumentException($"{id}에 해당되는 ID가 존재하지 않습니다.");
        }

        public T GetByName(string name)
        {
            if (nameToId.ContainsKey(name))
                return GetById(nameToId[name]);

            throw new ArgumentException($"{name}은(는) 존재하지 않습니다.");
        }

        public IEnumerable<T> GetAll()
        {
            return itemsById.Values;
        }
    }
}
