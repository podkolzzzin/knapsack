#define ISDLL
#include <iostream>
#include <Windows.h>
#include <string>
#include <vector>
#include <fstream>
#include <time.h>
#include <algorithm>
using namespace std;
namespace BitHelper
{
	bool IsBitSet(unsigned __int64 num,int bitNum)
	{
		return (1<<bitNum)&num;
	}

}
namespace Knapsack
{
	//declarations
	class Knapsack;
	class KnapsackSolution;
	class KnapsackIntarface
	{
	public:
		static void ShowTempSolution(KnapsackSolution solution);
		static Knapsack CollectInfo();
		static Knapsack CollectInfoFile(ifstream &file);
	};
	//classes
	struct KnapsackObject
	{
		string Name;
		int Weight;
		int Price;
		bool IsUsed;
		double Density;
		KnapsackObject()
		{
			IsUsed=true;
		}
		static KnapsackObject NullObject()
		{
			KnapsackObject t;
			t.Weight =- 1;
			t.Price =- 1;
			t.IsUsed=true;
			return t;
		}
		bool operator <(const KnapsackObject obj)
		{
			return this->Density<obj.Density;
		}
	};
	class KnapsackSolution
	{
		unsigned __int64 _mask;
		int _numObjectsInSolution,_totalWeight,_totalPrice;
		vector<KnapsackObject>* _objects;
		bool _isDefferent;
		int _prev;
	public:
		KnapsackSolution(unsigned __int64 mask,vector<KnapsackObject>* objects)
		{
			_mask=mask;
			_objects=objects;
			_prev=0;
			_totalPrice=0;
			_totalWeight=0;
			_numObjectsInSolution=0;
			for(int i=0;i<64;i++)
			{
				if((1<<i)&_mask && i<_objects->size())
				{
					_totalPrice+=objects->at(i).Price;
					_totalWeight+=objects->at(i).Weight;
					_numObjectsInSolution++;
				}
			}
		}
		KnapsackSolution(bool isDifferent,vector<KnapsackObject>* objects)
		{
			_mask=0;
			
			_isDefferent=true;
			_objects=objects;
			_prev=0;
			_totalPrice=0;
			_totalWeight=0;
			_numObjectsInSolution=0;
			for(int i=0;i<objects->size();i++)
			{
				if(_objects->at(i).IsUsed)
				{
					_totalPrice+=_objects->at(i).Price;
					_totalWeight+=_objects->at(i).Weight;
					_mask|=1<<i;
				}
			}
		}
		KnapsackSolution(vector<KnapsackObject>* objects)
		{
			_mask=0;
			_prev=0;
			_totalPrice=0;
			_totalWeight=0;
			_objects=objects;			
		}
		unsigned __int64 Mask()
		{
			return _mask;
		}
		void init(unsigned __int64 mask)
		{
			_totalPrice=0;
			_totalWeight=0;
			_mask=mask;
			for(int i=0;i<64;i++)
			{
				if((1<<i)&_mask && i<_objects->size())
				{
					_totalPrice+=_objects->at(i).Price;
					_totalWeight+=_objects->at(i).Weight;
					_numObjectsInSolution++;
				}
			}
		}
		int Length()
		{
			return _numObjectsInSolution;
		}
		KnapsackObject GetNextObject()
		{
			if(!_isDefferent)
			{
				for(int i=_prev;i<_objects->size();i++)
				{
					if(BitHelper::IsBitSet(_mask,i))
					{
						_prev=i+1;
						return _objects->at(i);
					}
				}
				_prev=0;
				for(int i=_prev;i<_objects->size();i++)
				{
					if(BitHelper::IsBitSet(_mask,i))
					{
						return _objects->at(i);
					}
				}
				return KnapsackObject::NullObject();
			}
			for(int i=_prev;i<_objects->size();i++)
			{
				if(_objects->at(i).IsUsed)
					return _objects->at(i);
			}
			_prev=0;
			for(int i=_prev;i<_objects->size();i++)
			{
				if(_objects->at(i).IsUsed)
					return _objects->at(i);
			}
			return KnapsackObject::NullObject();
		}
		int TotalPrice()
		{
			return _totalPrice;
		}
		int TotalWeight()
		{
			return _totalWeight;
		}
		static KnapsackSolution GenerateFromObjects(vector<KnapsackObject>* objects)
		{
			return KnapsackSolution(true,objects);
		}
	};
	struct TestResults
	{
		int FullV1Result,FullV1Time,FullV1Weight;
		int FullV2Result,FullV2Time,FullV2Weight;
		int GreedyResult,GreedyTime,GreedyWeight;
	};
	class Knapsack
	{
		vector<KnapsackObject>* _objects;
		int _maxWeight;
		int _maxFoundPrice;
		unsigned __int64 _maxPriceMask;
		void _findSolutionV1(unsigned __int64 _mask,int totalWeight,int totalPrice)
		{
			for(int i=0;i<_objects->size();i++)
			{
				if(!(_mask>>i)&1)
				{
					if(totalWeight + _objects->at(i).Weight <= _maxWeight)
					{
						totalPrice += _objects->at(i).Price;
						totalWeight += _objects->at(i).Weight;
						_mask |= (1<<i);
						if(totalPrice>_maxFoundPrice)
						{
							_maxFoundPrice=totalPrice;
							_maxPriceMask=_mask;
							//cout<<totalPrice<<endl;
							//KnapsackIntarface::ShowTempSolution(KnapsackSolution(_mask,_objects));
						}
						_findSolutionV1(_mask,totalWeight,totalPrice);

						totalPrice -= _objects->at(i).Price;
						totalWeight -= _objects->at(i).Weight;
						_mask &= ~(1<<i);
					}
				}
			}
		}
		void _findSolutionV2()
		{
			unsigned __int64 endFor= ((unsigned __int64)1)<<_objects->size();
			KnapsackSolution temp(_objects),best(_objects);
			for(unsigned __int64 i=0;i<=endFor;i++)
			{
				temp.init(i);
				if(temp.TotalWeight()>_maxWeight)
					continue;
				if(temp.TotalPrice()>best.TotalPrice())
				{
					_maxPriceMask=i;
					_maxFoundPrice=temp.TotalPrice();
					best=temp;
				}
			}
		}
		KnapsackSolution _findSolutionGreedyDirect()
		{
			int totalWeight=0;
			int totalPrice=0;
			for(int i=0;i<_objects->size();i++)
			{
				_objects->at(i).Density=(double)_objects->at(i).Price / _objects->at(i).Weight;
				_objects->at(i).IsUsed=false;
			}
			sort(_objects->rbegin(),_objects->rend()); // сортировка по убыванию
			int j=0;
			for(int i=0;i<_objects->size();i++)
			{
				if(totalWeight + _objects->at(i).Weight <=  _maxWeight)
				{
					_objects->at(i).IsUsed=true;
					totalWeight += _objects->at(i).Weight;
					totalPrice  += _objects->at(i).Price;
				}
			}
			return KnapsackSolution::GenerateFromObjects(_objects);
		}
	public:		
		Knapsack(int maxWeight)
		{
			_maxWeight=maxWeight;
			_objects=new vector<KnapsackObject>();
		}
		void AddObject(KnapsackObject obj)
		{
			if(obj.Weight<=_maxWeight)			
				_objects->push_back(obj);
		}
		int MaxWeight()
		{
			return _maxWeight;
		}
		KnapsackSolution FindSolutionFullSearchV1()
		{
			_maxFoundPrice = 0;
			_maxPriceMask = 0;
			_findSolutionV1(0,0,0);
			return KnapsackSolution(_maxPriceMask,_objects);
		}
		KnapsackSolution FindSolutionFullSearchV2()
		{
			_maxFoundPrice = 0;
			_maxPriceMask = 0;
			_findSolutionV2();
			return KnapsackSolution(_maxPriceMask,_objects);			
		}
		KnapsackSolution FindSolutionGreedySearchDirect()
		{
			_maxFoundPrice=0;
			return _findSolutionGreedyDirect();
		}
		TestResults Results;
		void SaveToFile(ofstream &file)
		{
			file<<_objects->size()<<endl;
			for(int i=0;i<_objects->size();i++)
				file<<_objects->at(i).Name<<" "<<_objects->at(i).Price<<" "<<_objects->at(i).Weight<<endl;
		}
		void SaveTestsToFile(ofstream &file)
		{
			file<<Size()<<" "
				<<Results.FullV1Result<<" "
				<<Results.FullV1Time<<" "
				<<Results.FullV1Weight<<" "
				<<Results.FullV2Result<<" "
				<<Results.FullV2Time<<" "
				<<Results.FullV2Weight<<" "
				<<Results.GreedyResult<<" "
				<<Results.GreedyTime<<" "
				<<Results.GreedyWeight<<endl;
		}
		void ShowTests()
		{
			cout<<"Knapsack capacity="<<_maxWeight<<" numObjects="<<Size()<<endl;	
			cout<<"FullV1 :"<<Results.FullV1Result<<"$ "<<Results.FullV1Time<<"msec "<<Results.FullV1Weight<<"kg"<<endl;
			cout<<"FullV2 :"<<Results.FullV2Result<<"$ "<<Results.FullV2Time<<"msec "<<Results.FullV2Weight<<"kg"<<endl;
			cout<<"Greedy :"<<Results.GreedyResult<<"$ "<<Results.GreedyTime<<"msec "<<Results.GreedyWeight<<"kg"<<endl;
			cout<<"============================="<<endl;
		}
		int Size()
		{
			return this->_objects->size();
		}
		bool operator <(const Knapsack& a)
		{
			return this->_objects->size()<a._objects->size();
		}

	};
	void KnapsackIntarface::ShowTempSolution(KnapsackSolution solution)
	{
		/*system("cls");
		cout<<"New temp solution found"<<endl;
		cout<<"Total price: "<<solution.TotalPrice()<<"$"<<endl;
		cout<<"Total weight: "<<solution.TotalWeight()<<endl;
		cout<<"Num objects: "<<solution.Length()<<endl;
		cout<<"Objects: "<<endl;
		for(int i=0;i<solution.Length();i++)
		{
		auto tObj=solution.GetNextObject();
		cout<<"\t"<<tObj.Name<<"(m="<<tObj.Weight<<" "<<tObj.Price<<"$)"<<endl;
		}*/
	}
	Knapsack KnapsackIntarface::CollectInfo()
	{
		cout<<"Maximum weight: ";
		int maxWeight;
		cin>>maxWeight;
		Knapsack a(maxWeight);
		int numObjects;
		cout<<"Num objects: ";
		cin>>numObjects;
		KnapsackObject t;
		cout<<"ObjectName Weight Price"<<endl;
		for(int i=0;i<numObjects;i++)
		{
			cin>>t.Name>>t.Weight>>t.Price;
			a.AddObject(t);
		}
		return a;
	}
	void UnitTests()
	{
		srand(time(0));
		ifstream tests;
		ofstream logForParse("logForParse.txt");
		ofstream logForRead("logForRead.txt");
		int start,end;
		vector<Knapsack> testData;
		ofstream testDataFile("testData.txt");
		for(int i=2;i<40;i++)
		{
			for(int j=0;j<100-i*2;j++)
			{
				Knapsack a(rand()%250+500);
				KnapsackObject temp;
				temp.Name="obj";
				for(int g=0;g<i;g++)
				{
					temp.Weight=rand()%30+20;
					temp.Price=rand()%50+25;			
					a.AddObject(temp);
				}
				testData.push_back(a);
				a.SaveToFile(testDataFile);
			}
		}
		sort(testData.begin(),testData.end());
		int num=0;
		for(int i=0;i<testData.size();i++)
		{
			start=clock();
			auto rez = testData[i].FindSolutionGreedySearchDirect();
			end=clock();
			testData[i].Results.GreedyResult=rez.TotalPrice();
			testData[i].Results.GreedyWeight=rez.TotalWeight();
			testData[i].Results.GreedyTime=end-start;
			cout<<"Greedy #"<<testData[i].Size()<<" "<<testData[i].Results.GreedyResult<<"$ "<<testData[i].Results.GreedyTime<<endl;

			//==full v1
			start=clock();
			rez = testData[i].FindSolutionFullSearchV1();
			end=clock();
			testData[i].Results.FullV1Result=rez.TotalPrice();
			testData[i].Results.FullV1Weight=rez.TotalWeight();
			testData[i].Results.FullV1Time=end-start;
			cout<<"FullV1 #"<<testData[i].Size()<<" "<<testData[i].Results.FullV1Result<<"$ "<<testData[i].Results.FullV1Weight<<endl;

			//==full v2
			start=clock();
			rez = testData[i].FindSolutionFullSearchV2();
			end=clock();
			testData[i].Results.FullV2Result=rez.TotalPrice();
			testData[i].Results.FullV2Weight=rez.TotalPrice();
			testData[i].Results.FullV2Time=end-start;
			cout<<"FullV2 #"<<testData[i].Size()<<" "<<testData[i].Results.FullV2Result<<"$ "<<testData[i].Results.FullV2Weight<<endl;

			//==
			testData[i].SaveTestsToFile(logForParse);
		}
	}
	Knapsack KnapsackIntarface::CollectInfoFile(ifstream &file)
	{
		int maxWeight;
		file>>maxWeight;
		Knapsack a(maxWeight);
		int numObjects;
		string tt;
		file>>numObjects;
		KnapsackObject t;
		for(int i=0;i<numObjects;i++)
		{
			file>>t.Name;
			file>>t.Weight;
			file>>t.Price;
			a.AddObject(t);
		}
		return a;
	}
};
unsigned __int64 FullSearchV1()
{
	ifstream file("cSharpBridge.txt");
	auto a=Knapsack::KnapsackIntarface::CollectInfoFile(file);
	return a.FindSolutionFullSearchV1().Mask();
}
unsigned __int64 FullSearchV2()
{
	ifstream file("cSharpBridge.txt");
	auto a=Knapsack::KnapsackIntarface::CollectInfoFile(file);
	return a.FindSolutionFullSearchV2().Mask();
}
unsigned __int64 GreedySearch()
{
	ifstream file("cSharpBridge.txt");
	auto a=Knapsack::KnapsackIntarface::CollectInfoFile(file);
	return a.FindSolutionGreedySearchDirect().Mask();
}
int main()
{
	ifstream file("cSharpBridge.txt");
	ofstream ofile("cPlusPlusBridge.txt");
	int alg;
	file>>alg;
	auto a=Knapsack::KnapsackIntarface::CollectInfoFile(file);
	if(alg==0)
	{
		ofile<<a.FindSolutionFullSearchV1().Mask();
	}
	else if(alg==1)
	{
		ofile<<a.FindSolutionFullSearchV2().Mask();
	}
	else
	{
		ofile<<a.FindSolutionGreedySearchDirect().Mask();
	}
	return 0;
}