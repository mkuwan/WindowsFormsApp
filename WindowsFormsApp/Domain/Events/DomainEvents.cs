using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp.Domain.Events
{
    /// <summary>
    /// イベントを購読して、ドメインイベントを発生させる静的クラス
    /// ドメイン駆動設計に固有のドメインイベントの概念を最初に導入したのはUdiDahanだったと思います。
    /// 考え方は単純です。ドメインにとって重要なイベントを示したい場合は、
    /// このイベントを明示的に発生させ、ドメインモデル内の他のクラスにサブスクライブさせてそれに反応させます。
    /// 
    /// Udi Dahan がもともと提案しているのは (たとえば、「Domain Events – Take 2」
    /// (ドメイン イベント – テイク 2 などの複数の関連する投稿を参照))、
    /// イベントの管理と生成に静的クラスを使う方法です。
    /// これには、DomainEvents.Raise(Event myEvent) のような構文を使用し、
    /// 呼び出されるとすぐにドメイン イベントを生成する DomainEvents という名前の静的クラスが含まれる場合があります。
    /// Jimmy Bogard も、ブログ投稿「Strengthening your domain:Domain Events」
    /// (ドメインの強化: ドメイン イベント) で同様のアプローチを推奨しています。
    ///
    /// ドメイン イベント クラスが静的である場合は、ハンドラーにもすぐにディスパッチします
    /// </summary>
    public static class DomainEvents
    {
        private static List<Delegate> _actions;
        
        /// <summary>
        /// Event Handler登録: +=
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="eventHandler"></param>
        public static void Register<T>(Action<T> eventHandler) where T : IDomainEvent
        {
            _actions = _actions ?? new List<Delegate>();

            _actions.Add(eventHandler);
        }

        /// <summary>
        /// Domain Event発行(Publish)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="domainEvent"></param>
        public static void Raise<T>(T domainEvent) where T : IDomainEvent
        {
            foreach (Delegate action in _actions)
            {
                if (action is Action<T> act)
                {
                    //act(domainEvent);
                    act.Invoke(domainEvent);
                }
                
            }
        }

        public static void ClearAllEvents()
        {
            _actions.Clear();
        }

        /// <summary>
        /// Event Handler解除: -=
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="eventHandler"></param>
        public static void Release<T>(Action<T> eventHandler) where T : IDomainEvent
        {
            _actions.Remove(eventHandler);
        }
    }
}
