using System;
using System.Collections.Generic;

namespace Game.Scripts.Common.DataStructures.Heap
{
    public abstract class BaseHeap
    {
	    protected int nodesCount;

	    protected int MaxSize;
	    protected float[] Heap;
	    protected float TempHeap;
	    
	    public int Count => nodesCount;
	    public float HeadValue => Heap[1];
	    
        protected BaseHeap(int initialSize)
		{
			MaxSize = initialSize;
			Heap = new float[initialSize + 1];
		}
        
		public void Clear() => 
			nodesCount = 0;

		protected int Parent(int index) => 
			index >> 1;

		protected int Left(int index) => 
			index << 1;

		protected int Right(int index) => 
			index << 1 | 1;

		protected virtual void BubbleDownMax(int index)
		{
			int L = Left(index);
			for (int R = Right(index); R <= nodesCount; R = Right(index))
			{
				bool flag = Heap[index] < Heap[L];
				if (flag)
				{
					bool flag2 = Heap[L] < Heap[R];
					if (flag2)
					{
						Swap(index, R);
						index = R;
					}
					else
					{
						Swap(index, L);
						index = L;
					}
				}
				else
				{
					bool flag3 = Heap[index] < Heap[R];
					if (!flag3)
					{
						index = L;
						L = Left(index);
						break;
					}
					Swap(index, R);
					index = R;
				}
				L = Left(index);
			}
			bool flag4 = L <= nodesCount && Heap[index] < Heap[L];
			if (flag4)
			{
				Swap(index, L);
			}
		}
		
		protected virtual void BubbleUpMax(int index)
		{
			int P = Parent(index);
			while (P > 0 && Heap[P] < Heap[index])
			{
				Swap(P, index);
				index = P;
				P = Parent(index);
			}
		}
		
		protected virtual void BubbleDownMin(int index)
		{
			int L = Left(index);
			for (int R = Right(index); R <= nodesCount; R = Right(index))
			{
				bool flag = Heap[index] > Heap[L];
				if (flag)
				{
					bool flag2 = Heap[L] > Heap[R];
					if (flag2)
					{
						Swap(index, R);
						index = R;
					}
					else
					{
						Swap(index, L);
						index = L;
					}
				}
				else
				{
					bool flag3 = Heap[index] > Heap[R];
					if (!flag3)
					{
						index = L;
						L = Left(index);
						break;
					}
					Swap(index, R);
					index = R;
				}
				L = Left(index);
			}
			bool flag4 = L <= nodesCount && Heap[index] > Heap[L];
			if (flag4)
			{
				Swap(index, L);
			}
		}
		
		protected virtual void BubbleUpMin(int index)
		{
			int P = Parent(index);
			while (P > 0 && Heap[P] > Heap[index])
			{
				Swap(P, index);
				index = P;
				P = Parent(index);
			}
		}
		
		protected virtual void Swap(int A, int B)
		{
			TempHeap = Heap[A];
			Heap[A] = Heap[B];
			Heap[B] = TempHeap;
		}

		protected virtual void UpsizeHeap()
		{
			MaxSize *= 2;
			Array.Resize<float>(ref Heap, MaxSize + 1);
		}
		
		public virtual void PushValue(float h) => 
			throw new NotImplementedException();
		
		public virtual float PopValue() => 
			throw new NotImplementedException();
		
		public void FlushHeapResult(List<float> heapList)
		{
			for (int i = 1; i < Count; i++) 
				heapList.Add(Heap[i]);
		}
    }
}