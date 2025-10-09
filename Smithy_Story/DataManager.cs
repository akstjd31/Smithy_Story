using System.Collections.Generic;
using System;
using System.Threading;

// 제네릭 데이터 관리자 클래스 (데이터 저장/조회)
public class DataManager<T> where T : IData
{
    private Dictionary<int, T> itemsById = new Dictionary<int, T>();
    private Dictionary<string, int> nameToId = new Dictionary<string, int>();

    // 데이터 추가
    public void AddItem(T item)
    {
        if (!itemsById.ContainsKey(item.ID))
        {
            itemsById[item.ID] = item;
            nameToId[item.Name] = item.ID;
        }
    }

    // ID 기반 데이터 검색
    public T GetById(int id)
    {
        if (itemsById.ContainsKey(id))
            return itemsById[id];

        throw new ArgumentException($"{id}에 해당되는 ID가 존재하지 않습니다.");
    }

    // Name 기반 데이터 검색
    public T GetByName(string name)
    {
        if (nameToId.ContainsKey(name))
            return GetById(nameToId[name]);


        throw new ArgumentException($"{name}은(는) 존재하지 않습니다.");
    }

    // 전체 데이터 출력
    public IEnumerable<T> GetAll() => itemsById.Values;
}
