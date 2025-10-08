using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Smithy_Story
{
    public class WeaponData
    {
        // 상수
        // 변수
        private static DataManager<Weapon> manager = new DataManager<Weapon>();

        // 프로퍼티

        // 생성자
        // 생성자
        static WeaponData()
        {
            // ---------------------- Common 무기 ----------------------
            manager.AddItem(new Weapon(id: 1, name: "곡괭이", price: 20, grade: Grade.Common, craftMinutes: 10,
                requiredResources: new Dictionary<Resource, int> {
                    { ResourceData.GetByName("돌"), 3 },
                    { ResourceData.GetByName("나무"), 1 }
                }));

            manager.AddItem(new Weapon(id: 2, name: "목검", price: 30, grade: Grade.Common, craftMinutes: 20,
                requiredResources: new Dictionary<Resource, int> {
                    { ResourceData.GetByName("나무"), 3 },
                    { ResourceData.GetByName("가죽"), 1 }
                }));
            
            manager.AddItem(new Weapon(id: 3, name: "철검", price: 30, grade: Grade.Common, craftMinutes: 25,
                requiredResources: new Dictionary<Resource, int> {
                    { ResourceData.GetByName("철"), 3 },
                    { ResourceData.GetByName("가죽"), 1 }
                }));

            manager.AddItem(new Weapon(id: 4, name: "가죽활", price: 40, grade: Grade.Common, craftMinutes: 30,
                requiredResources: new Dictionary<Resource, int> {
                    { ResourceData.GetByName("나무"), 2 },
                    { ResourceData.GetByName("실"), 1 },
                    { ResourceData.GetByName("가죽"), 1 }
                }));


            // ---------------------- Rare 무기 ----------------------
            manager.AddItem(new Weapon(id: 51, name: "단검", price: 80, grade: Grade.Rare, craftMinutes: 40,
                requiredResources: new Dictionary<Resource, int> {
                    { ResourceData.GetByName("구리"), 3 },
                    { ResourceData.GetByName("나무"), 2 }
                }));

            manager.AddItem(new Weapon(id: 52, name: "철제도끼", price: 100, grade: Grade.Rare, craftMinutes: 50,
                requiredResources: new Dictionary<Resource, int> {
                    { ResourceData.GetByName("철"), 3 },
                    { ResourceData.GetByName("가죽"), 1 }
                }));

            manager.AddItem(new Weapon(id: 53, name: "은장검", price: 130, grade: Grade.Rare, craftMinutes: 60,
                requiredResources: new Dictionary<Resource, int> {
                    { ResourceData.GetByName("은광석"), 3 },
                    { ResourceData.GetByName("실"), 1 }
                }));


            // ---------------------- Epic 무기 ----------------------
            manager.AddItem(new Weapon(id: 71, name: "금제창", price: 200, grade: Grade.Epic, craftMinutes: 90,
                requiredResources: new Dictionary<Resource, int> {
                    { ResourceData.GetByName("금광석"), 3 },
                    { ResourceData.GetByName("은광석"), 2 },
                    { ResourceData.GetByName("나무"), 1 }
                }));

            manager.AddItem(new Weapon(id: 72, name: "흑요석검", price: 250, grade: Grade.Epic, craftMinutes: 120,
                requiredResources: new Dictionary<Resource, int> {
                    { ResourceData.GetByName("흑요석"), 2 },
                    { ResourceData.GetByName("철"), 3 },
                    { ResourceData.GetByName("가죽"), 2 }
                }));

            manager.AddItem(new Weapon(id: 73, name: "루비스태프", price: 300, grade: Grade.Epic, craftMinutes: 150,
                requiredResources: new Dictionary<Resource, int> {
                    { ResourceData.GetByName("루비"), 2 },
                    { ResourceData.GetByName("불의 정수"), 1 },
                    { ResourceData.GetByName("천"), 1 }
                }));


            // ---------------------- Legendary 무기 ----------------------
            manager.AddItem(new Weapon(id: 91, name: "용린검", price: 450, grade: Grade.Legendary, craftMinutes: 200,
                requiredResources: new Dictionary<Resource, int> {
                    { ResourceData.GetByName("용의 비늘"), 2 },
                    { ResourceData.GetByName("마력의 핵"), 1 },
                    { ResourceData.GetByName("철"), 2 }
                }));

            manager.AddItem(new Weapon(id: 92, name: "고대의대검", price: 600, grade: Grade.Legendary, craftMinutes: 240,
                requiredResources: new Dictionary<Resource, int> {
                    { ResourceData.GetByName("고대 금속"), 2 },
                    { ResourceData.GetByName("에메랄드"), 1 },
                    { ResourceData.GetByName("마력의 핵"), 1 }
                }));

            manager.AddItem(new Weapon(id: 93, name: "다이아몬드창", price: 700, grade: Grade.Legendary, craftMinutes: 300,
                requiredResources: new Dictionary<Resource, int> {
                    { ResourceData.GetByName("다이아몬드"), 2 },
                    { ResourceData.GetByName("에메랄드"), 1 },
                    { ResourceData.GetByName("실"), 1 }
                }));
        }

        // 메소드
        public static Weapon GetById(int id) => manager.GetById(id);  
        public static Weapon GetByName(string name) => manager.GetByName(name);
        public static IEnumerable<Weapon> GetAll() => manager.GetAll();
    }
}
