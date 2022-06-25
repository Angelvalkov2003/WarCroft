using System;

using WarCroft.Constants;
using WarCroft.Entities.Inventory;
using WarCroft.Entities.Items;

namespace WarCroft.Entities.Characters.Contracts
{
	public abstract class Character
	{
		private string name;
		private double health;
        private double armor;

        public Character(string name, double health, double armor, double abilityPoints, Bag bag)
        {
            Name = name;
            AbilityPoints = abilityPoints;
            Bag = bag;
            Armor = armor;
            Health = health;
            BaseHealth = health;
            BaseArmor = armor;
            
        }

        public double AbilityPoints { get; set; }
        public IBag Bag { get; set; }
        public double BaseHealth { get; private set; }
        public double BaseArmor { get; private set; }


        public double Armor
        {
            get { return armor; }
            set { armor = value;

                if (armor < 0)
                {
                    armor = 0;
                }
                
            }
        }

        public string Name
		{
			get { return this.name; }
			private set
			{
				if (string.IsNullOrWhiteSpace(value) )
                {
					throw new ArgumentException(ExceptionMessages.CharacterNameInvalid);
                }
				name = value; 
			}
        }

		public double Health
        {
            get { return health; }
            set { health = value;
                if (health<0)
                {
					health = 0;
                    this.IsAlive = false;
                }
                if (health > BaseHealth)
                {
					health = BaseHealth;
                }
			}
        }


        public bool IsAlive { get; set; } = true;

		protected void EnsureAlive()
		{
			if (!this.IsAlive)
			{
				throw new InvalidOperationException(ExceptionMessages.AffectedCharacterDead);
			}
		}

        public void TakeDamage(double hitPoints)
        {
            if (!this.IsAlive)
            {
                return;
            }

            double LeftPoints = hitPoints - this.Armor;

            if (LeftPoints < 0)
            {
                LeftPoints = 0;
            }
            this.Armor -= hitPoints;
            this.Health -= LeftPoints;

            if (this.Health == 0)
            {
                this.IsAlive = false;
            }
        }

        public void UseItem(Item item)
        {
            if (!this.IsAlive)
            {
                return;
            }

            item.AffectCharacter(this);
        }

    }
}