using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WarCroft.Constants;
using WarCroft.Entities.Characters;
using WarCroft.Entities.Characters.Contracts;
using WarCroft.Entities.Items;

namespace WarCroft.Core
{
	public class WarController
	{
		private readonly List<Character> characters;
		private readonly List<Item> items;

		public WarController()
		{
			this.characters = new List<Character>();
			this.items = new List<Item>();
		}

		public string JoinParty(string[] args)
		{
			string name = args[1];
			string characterType = args[0];

			Character character = null;
            if (characterType == "Warrior")
            {
				character = new Warrior(name);
            }
            else if (characterType == "Priest")
            {
				character = new Priest(name);
			}
            else
            {
				throw new ArgumentException(string.Format(ExceptionMessages.InvalidCharacterType, name));
            }
			this.characters.Add(character);
			return String.Format(SuccessMessages.JoinParty, name);
		}

		public string AddItemToPool(string[] args)
		{
			string itemName = args[0];

			Item item = null;

            if (itemName == nameof(FirePotion))
            {
				item = new FirePotion();
            }
			else if (itemName == nameof(HealthPotion))
			{
				item = new HealthPotion();
			}
            else
            {
				throw new ArgumentException(string.Format(ExceptionMessages.InvalidItem, itemName));
            }

			this.items.Add(item);
			return String.Format(SuccessMessages.AddItemToPool, itemName);

		}

		public string PickUpItem(string[] args)
		{
			string characterName = args[0];
			Character character = this.characters.FirstOrDefault(x => x.Name == characterName);

            if (character == null)
            {
				throw new ArgumentException(string.Format(ExceptionMessages.CharacterNotInParty, characterName));
            }
            if (!items.Any())
            {
				throw new InvalidOperationException(ExceptionMessages.ItemPoolEmpty);
            }
			Item item = this.items.LastOrDefault();
			this.items.Remove(item);
			character.Bag.AddItem(item);
			return string.Format(SuccessMessages.PickUpItem,characterName, item.GetType().Name);
		}

		public string UseItem(string[] args)
		{
			string characterName = args[0];
			string itemName = args[1];

			Character character = this.characters.FirstOrDefault(x => x.Name == characterName);

			if (character == null)
			{
				throw new ArgumentException(string.Format(ExceptionMessages.CharacterNotInParty, characterName));
			}
			Item item = character.Bag.GetItem(itemName);

			character.UseItem(item);

			return string.Format(SuccessMessages.UsedItem,characterName, itemName);

		}

		public string GetStats()
		{
			StringBuilder sb = new StringBuilder();
            foreach (Character character in this.characters.OrderByDescending(x => x.IsAlive).ThenByDescending(x=>x.Health))
            {
				sb.Append(String.Format
					(SuccessMessages.CharacterStats,character.Name, character.Health, 
					character.BaseHealth, character.Armor, character.BaseHealth, 
					(character.IsAlive? "Alive" : "Dead")));

                Console.WriteLine();
            }
			return sb.ToString().TrimEnd();
		}

		public string Attack(string[] args)
		{
			string attackerName = args[0];
			string receiverName = args[1];

			Character attacker = this.characters.FirstOrDefault(x => x.Name == attackerName);
			Character receiver = this.characters.FirstOrDefault(x => x.Name == receiverName);

			if (attacker == null)
            {
				throw new ArgumentException(string.Format(ExceptionMessages.CharacterNotInParty, attackerName));
            }
            if (receiver == null)
            {
				throw new ArgumentException(string.Format(ExceptionMessages.CharacterNotInParty, receiverName));
			}
			if (!(attacker is IAttacker))
			{
				throw new ArgumentException($"{attackerName} cannot attack!");
			}

			((IAttacker)attacker).Attack(receiver);
			string result = string.Format(SuccessMessages.AttackCharacter, attackerName, receiverName,
				attacker.AbilityPoints, receiverName, receiver.Health,
				receiver.BaseHealth, receiver.Armor, receiver.BaseArmor);

            if (!receiver.IsAlive)
            {
				result += Environment.NewLine +
					String.Format(SuccessMessages.AttackKillsCharacter, receiverName);
            }
			return result;
        }

		public string Heal(string[] args)
		{
			string healerName = args[0];
			string receiverName = args[1];

			Character healer = this.characters.FirstOrDefault(x => x.Name == healerName);
			Character receiverheal = this.characters.FirstOrDefault(x => x.Name == receiverName);

			if (healer == null)
			{
				throw new ArgumentException(string.Format(ExceptionMessages.CharacterNotInParty, healerName));
			}
			if (receiverheal == null)
			{
				throw new ArgumentException(string.Format(ExceptionMessages.CharacterNotInParty, receiverName));
			}

            if (!(healer is IHealer))
            {
				throw new ArgumentException($"{healerName} cannot heal!");
            }

			((IHealer)healer).Heal(receiverheal);

			string result = string.Format(SuccessMessages.HealCharacter, healerName, receiverName,
				healer.AbilityPoints, receiverName, receiverheal.Health);

			return result;
			
		}
	}
}
