using System.Collections.Generic;

namespace Game1
{
    public class PatrolEnemySet
    {
        private List<PatrolEnemy> mTheSet = new List<PatrolEnemy>();
        const int DEFAULT_NUM_ENEMIES = 15;

        public PatrolEnemySet(int numEnemies = DEFAULT_NUM_ENEMIES)
        {
            CreateEnemySet(numEnemies);
        }

        private void CreateEnemySet(int numEnemies)
        {
            for (int i = 0; i < numEnemies; i++)
            {
                PatrolEnemy enemy = new PatrolEnemy();
                mTheSet.Add(enemy);
            }
        }

        public int UpdateSet(GameObject hero)
        {
            int count = 0;
            foreach (var enemy in mTheSet)
            {
                if (enemy.UpdatePatrol(hero))
                    count++;
            }
            return count;
        }

        public void DrawSet()
        {
            foreach (var enemy in mTheSet)
                enemy.Draw();
        }
    }
}
