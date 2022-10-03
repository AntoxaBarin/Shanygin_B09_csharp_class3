// Колода

using System.Collections.Concurrent;
using System.Runtime.InteropServices.ComTypes;
using Deck = System.Collections.Generic.List<Card>;
// Набор карт у игрока
using Hand = System.Collections.Generic.List<Card>;
// Набор карт, выложенных на стол
using Table = System.Collections.Generic.List<Card>;

// Масть
internal enum Suit
{
    clubs,
    diamonds,
    spades,
    hearts
}

// Значение
internal enum Rank
{
    six = 0,
    seven,
    eight,
    nine,
    ten,
    jack,
    queen,
    king,
    ace
}

// Карта
internal record struct Card(Rank Rank, Suit Suit);

internal record struct RoundResult(Player? Player, Table Table);

// Тип для обозначения игрока (первый, второй)
internal enum Player
{
    FirstPlayer,
    SecondPlayer
}

namespace Task1
{
    public static class Task1
    {
        private const int MaxRounds = 300;

        /*
        * Реализуйте игру "Пьяница" (в простейшем варианте, на колоде в 36 карт)
        * https://ru.wikipedia.org/wiki/%D0%9F%D1%8C%D1%8F%D0%BD%D0%B8%D1%86%D0%B0_(%D0%BA%D0%B0%D1%80%D1%82%D0%BE%D1%87%D0%BD%D0%B0%D1%8F_%D0%B8%D0%B3%D1%80%D0%B0)
        * Рука — это набор карт игрока. Карты выкладываются на стол из начала "рук" и сравниваются
        * только по значениям (масть игнорируется). При равных значениях сравниваются следующие карты.
        * Набор карт со стола перекладывается в конец руки победителя. Шестерка туза не бьёт.
        *
        * Реализация должна сопровождаться тестами.
        */

        // Размер колоды
        internal const int DeckSize = 36;

        // Возвращается null, если значения карт совпадают
        internal static Player? RoundWinner(Card card1, Card card2)
        {
            Player? winner = null;
            if (card1.Rank == card2.Rank)
            {
                winner = null;
            }
            else if (card1.Rank > card2.Rank)
            {
                winner = Player.FirstPlayer;
            }
            else if (card1.Rank < card2.Rank)
            {
                winner = Player.SecondPlayer;
            }

            return winner;
        }

// Возвращает полную колоду (36 карт) в фиксированном порядке
        internal static Deck FullDeck()
        {
            var deck = new List<Card>();      // Колода карт (полная 36 штук)

            foreach (Rank i in Enum.GetValues<Rank>())
            {
                foreach (Suit j in Enum.GetValues<Suit>())
                {
                    Card card = new Card(i, j);
                    deck.Add(card);
                }
            }

            return deck;
        }

        private static Card Pop(this Hand hand)            // Адаптация метода Pop для коллекции List
        {
            Card result = hand.Last();
            hand.RemoveAt(hand.Count - 1);
            return result;      
        }
        
// Раздача карт: случайное перемешивание (shuffle) и деление колоды пополам
        internal static Dictionary<Player, Hand> Deal(Deck deck)
        {
            Random random = new Random();
            for (int i = 35; i >= 1; i--)
            {
                int j = random.Next(i + 1);

                Card temp = deck[j];
                deck[j] = deck[i];
                deck[i] = temp;
            }

            // Теперь deck - перемешанная колода

            Dictionary<Player, Hand> dict = new Dictionary<Player, Hand>()
            {
                { Player.FirstPlayer, deck.GetRange(0, DeckSize / 2)},
                { Player.SecondPlayer, deck.GetRange(DeckSize / 2, (DeckSize + 1) / 2)}
            };
            
            return dict;
        }
        
        private static readonly ConcurrentBag<Table> AllTables = new ConcurrentBag<Table>();

        private static void FreeTable(Table table)
        {
            if (AllTables.Count < 100) AllTables.Add(table);
        }
        private static Table GetTable()
        {
            return AllTables.TryTake(out var result) ? result : new Table();
        } 

// Один раунд игры (в том числе спор при равных картах).
// Возвращается победитель раунда и набор карт, выложенных на стол.
        internal static RoundResult Round(Dictionary<Player, Hand> hands)
        {
            var hand1 = hands[Player.FirstPlayer];        // Руки игроков
            var hand2 = hands[Player.SecondPlayer];

            var table = GetTable();
            
            table.Add(hand1.Pop());           // Игроки кидают карты на стол, карты исчезают из рук игроков
            table.Add(hand2.Pop());
            
            var winner = RoundWinner(table[0], table[1]);       // определяем победителся раунда

            while (winner == null)                   // Если карты равны, то решаем "спор"
            {
                if (hand1.Count == 0 && hand2.Count == 0)      // Если руки у игроков пусты, то ничья
                {
                    winner = null;
                    break;
                }

                if (hand1.Count == 0)                    // Если карт у первого игрока нет, то он проиграл
                {
                    winner = Player.FirstPlayer;
                    break;
                }
                else if (hand2.Count == 0)                // Если карт у второго игрока нет, то он проиграл
                {
                    winner = Player.SecondPlayer;
                    break;
                }                  

                Card card1 = hand1.Pop();                 
                Card card2 = hand2.Pop();

                table.Add(card1);
                table.Add(card2);

                winner = RoundWinner(card1, card2);        // игроки выбрасывают по карте и решается победитель раунда
            }
            
            return new RoundResult(winner, table);        
        }

        


// Полный цикл игры (возвращается победивший игрок)
// в процессе игры печатаются ходы
        internal static Player? Game(Dictionary<Player, Hand> hands)
        {
            var deck = hands[Player.FirstPlayer].Concat(hands[Player.SecondPlayer]).ToList();
            
            for (int round = 0; round < MaxRounds; round++)
            {
                var winner = GamePart(hands);

                if (winner != null) return winner;
                else
                {
                    hands = Deal(deck);
                }
            }

            return null;
        }

        private static void Move<Card>(this List<Card> to, List<Card> from)
        {
            from.AddRange(to);
            to.Clear();
            
            to.AddRange(from);
            from.Clear();
        }

        private static Player? GamePart(Dictionary<Player, Hand> hands)
        {
            int round = 0;

            while (hands[Player.FirstPlayer].Count > 0 && hands[Player.SecondPlayer].Count > 0)       // Пока есть карты раунды прибавляются
            {
                var (winner, table) = Round(hands);

                if (winner == Player.FirstPlayer)
                {
                    hands[Player.FirstPlayer].Move(table);
                    FreeTable(table);
                }
                else if (winner == Player.SecondPlayer)
                {
                    hands[Player.SecondPlayer].Move(table);
                    FreeTable(table);
                }
                else 
                {
                    hands[Player.FirstPlayer].Move(table);
                    FreeTable(table);
                    return null;
                }

                round += 1;

                if (round > MaxRounds) return null;
            }

            if (hands[Player.FirstPlayer].Count == 0) return Player.SecondPlayer;     // выводим победителя
            if (hands[Player.SecondPlayer].Count == 0) return Player.FirstPlayer;

            throw new ArgumentException("Unreachable code");
        }

        public static void Main(string[] args)
        {
            var deck = FullDeck();
            var hands = Deal(deck);
            var winner = Game(hands);
            Console.WriteLine("Результат игры:");

            switch (winner)
            {
                case Player.FirstPlayer:
                    Console.WriteLine("Победил Игрок 1");
                    break;
                case Player.SecondPlayer:
                    Console.WriteLine("Победил Игрок 2");
                    break;
                case null:
                    Console.WriteLine("Ничья");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
        }
    }
}
