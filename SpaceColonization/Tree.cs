using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace TreeGenTest.SpaceColonization
{
    public class Tree
    {
        public bool DoneGrowing = false;

        public List<Leaf> Leaves;
        public List<Leaf> DeadLeaves;
        public Branch Root;
        public List<Branch> Branches;

        public float TrunkHeight = 5;
        public float MinDistance = 10;
        public float MaxDistance = 250;
        public float BranchLength = 5;

        public float ThicknessIncreaseFactor = 1.025f;

        public Tree(List<Leaf> leaves, Branch root)
        {
            Leaves = leaves;
            Root = root;

            CreateTrunk();
        }

        private void CreateTrunk()
        {
            DoneGrowing = false;

            DeadLeaves = new List<Leaf>();

            Branches = new List<Branch>();

            Branches.Add(Root);

            // Create a new branch on top of the root
            Branch current = new Branch(Root, new Vector2(Root.Position.X, Root.Position.Y + BranchLength), Root.Direction);
            Branches.Add(current);

            while ((Root.Position - current.Position).Length() < TrunkHeight)
            {
                Branch trunk = new Branch(current, new Vector2(current.Position.X, current.Position.Y + BranchLength), Root.Direction);
                Branches.Add(trunk);
                current = trunk;
            }
        }

        public void Grow()
        {
            if (DoneGrowing) return;

            if (Leaves.Count == 0)
            {
                DoneGrowing = true;
                return;
            }

            for (int i = 0; i < Leaves.Count; i++)
            {
                bool leafRemoved = false;

                Leaves[i].Closest = null;
                var direction = Vector2.Zero;

                foreach(var b in Branches)
                {
                    direction = Leaves[i].Position - b.Position;
                    float distance = (float)Math.Round(direction.Length());
                    direction.X /= direction.Length();
                    direction.Y /= direction.Length();

                    if (distance <= MinDistance)
                    {
                        DeadLeaves.Add(Leaves[i]);
                        Leaves.Remove(Leaves[i]);
                        i--;
                        leafRemoved = true;
                        break;
                    }
                    else if (distance <= MaxDistance)
                    {
                        if (Leaves[i].Closest == null)
                            Leaves[i].Closest = b;
                        else if ((Leaves[i].Position - Leaves[i].Closest.Position).Length() > distance)
                            Leaves[i].Closest = b;
                    }
                }

                if (!leafRemoved)
                {
                    if (Leaves[i].Closest != null)
                    {
                        Vector2 dir = Leaves[i].Position - Leaves[i].Closest.Position;
                        dir.X /= dir.Length();
                        dir.Y /= dir.Length();
                        Leaves[i].Closest.Direction += dir;
                        Leaves[i].Closest.GrowCount++;
                    }
                }
            }

            List<Branch> newBranches = new List<Branch>();
            foreach (Branch b in Branches)
            {
                b.Thickness *= ThicknessIncreaseFactor;

                if (b.GrowCount > 0)
                {
                    Vector2 avgDirection = b.Direction / b.GrowCount;
                    avgDirection.X /= avgDirection.Length();
                    avgDirection.Y /= avgDirection.Length();

                    Branch newBranch = new Branch(b, b.Position + avgDirection * BranchLength, avgDirection);

                    newBranches.Add(newBranch);
                    b.Reset();
                }
            }

            bool BranchAdded = false;
            foreach (Branch b in newBranches)
            {
                if (Branches.Where(x => x.Position == b.Position).Count() == 0)
                {
                    Branches.Add(b);
                    BranchAdded = true;
                }
            }

            if (!BranchAdded) DoneGrowing = true;
        }
    }
}
