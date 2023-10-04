using System;
using System.Collections.Generic;

namespace Game.Scripts.Common.DataStructures.Heap
{
    public class PriorityHeap<T> : BaseHeap where T : IPriorityItem
    {
	    public T[] Objs;
	    
	    private float[] _offsets;
	    private T _tempObj;
	    private float _tempOffset;
	    
	    public T HeadHeapObject => Objs[1];
	    
        public PriorityHeap(int maxNodes = 2048) : base(maxNodes)
		{
			Objs = new T[maxNodes + 1];
			_offsets = new float[maxNodes + 1];
		}
        
		protected override void Swap(int A, int B)
		{
			TempHeap = Heap[A];
			_tempObj = Objs[A];
			_tempOffset = _offsets[A];
			Heap[A] = Heap[B];
			Objs[A] = Objs[B];
			_offsets[A] = _offsets[B];
			Heap[B] = TempHeap;
			Objs[B] = _tempObj;
			_offsets[B] = _tempOffset;
		}
		
		public override void PushValue(float h) => 
			throw new ArgumentException("Use PushObj(T, float)!");
		
		public override float PopValue() => 
			throw new ArgumentException("Use Push(T, float)!");
		
		public void PushObj(T obj)
		{
			bool flag = nodesCount == MaxSize;
			if (flag)
			{
				UpsizeHeap();
			}
			
			nodesCount++;
			Heap[nodesCount] = obj.Priority;
			Objs[nodesCount] = obj;
			_offsets[nodesCount] = obj.Offset;
			BubbleUpMax(nodesCount);
		}
		
		public T PopObj()
		{
			bool flag = nodesCount == 0;
			if (flag)
			{
				throw new ArgumentException("Heap is empty!");
			}
			T result = Objs[1];
			Heap[1] = Heap[nodesCount];
			Objs[1] = Objs[nodesCount];
			_offsets[1] = _offsets[nodesCount];
			Objs[nodesCount] = default(T);
			_offsets[nodesCount] = 0f;
			nodesCount--;
			BubbleDownMax(1);
			return result;
		}
		
		public T PopObj(ref float heapValue)
		{
			bool flag = nodesCount == 0;
			if (flag)
			{
				throw new ArgumentException("Heap is empty!");
			}
			heapValue = Heap[1];
			return PopObj();
		}
		
		protected override void UpsizeHeap()
		{
			MaxSize *= 2;
			Array.Resize<float>(ref Heap, MaxSize + 1);
			Array.Resize<T>(ref Objs, MaxSize + 1);
			Array.Resize<float>(ref _offsets, MaxSize + 1);
		}
		
		public void FlushResult(List<T> resultList, List<float> heapList = null)
		{
			int count = nodesCount + 1;
			bool flag = heapList == null;
			if (flag)
			{
				for (int i = 1; i < count; i++)
				{
					resultList.Add(PopObj());
				}
			}
			else
			{
				float h = 0f;
				for (int j = 1; j < count; j++)
				{
					resultList.Add(PopObj(ref h));
					heapList.Add(h);
				}
			}
		}
		
		protected override void BubbleUpMin(int index) => 
			throw new NotImplementedException();
		
		protected override void BubbleDownMin(int index) => 
			throw new NotImplementedException();
		
		protected override void BubbleUpMax(int index)
		{
			int P = Parent(index);
			while (P > 0 && Compare(P, index))
			{
				Swap(P, index);
				index = P;
				P = Parent(index);
			}
		}
		
		protected override void BubbleDownMax(int index)
		{
			int L = Left(index);
			for (int R = Right(index); R <= nodesCount; R = Right(index))
			{
				bool flag = Compare(index, L);
				if (flag)
				{
					bool flag2 = Compare(L, R);
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
					bool flag3 = Compare(index, R);
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
			bool flag4 = L <= nodesCount && Compare(index, L);
			if (flag4)
			{
				Swap(index, L);
			}
		}
		
		private bool Compare(int L, int R) => 
			Heap[L] < Heap[R] || (Heap[L] == Heap[R] && _offsets[L] > _offsets[R]);
		
    }
}