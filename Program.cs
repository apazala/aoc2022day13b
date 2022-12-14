﻿internal class Program
{
    abstract class Signal:IComparable<Signal>
    {
        public static Signal CreateSignal(string s, ref int i)
        {   
            if(s[i] == ',')
                i++;         
            bool createArr = (s[i] == '[');
            if(createArr){
                i++;
                return new SignalArr(s, ref i);
            }
            else
                return new SignalInt(s, ref i);
        }

        public abstract int CompareTo(Signal? other);
    }

    class SignalArr:Signal
    {
        private List<Signal> itemsList;

        public SignalArr(SignalInt sig)
        {
            itemsList = new List<Signal>();
            itemsList.Add(sig);
        }
        public SignalArr(string s, ref int i)
        {
            itemsList = new List<Signal>();
            while(s[i] != ']')
            {
                itemsList.Add(Signal.CreateSignal(s, ref i));
            }
            i++;//skip ']'
        }

        public override int CompareTo(Signal? other)
        {
            SignalArr? sig2;
            if(other is SignalInt){
                sig2 = new SignalArr((SignalInt)other);
            } else {
                sig2 = (SignalArr)other;
            }

            int cmp = 0;
            int count = (itemsList.Count < sig2.itemsList.Count? itemsList.Count:sig2.itemsList.Count);
            for(int i = 0; cmp == 0 && i < count; i++)
            {
                cmp = itemsList[i].CompareTo(sig2.itemsList[i]);
            }

            return (cmp != 0? cmp : itemsList.Count - sig2.itemsList.Count);
        }

        public override string ToString()
        {
            string str = "[";
            if(itemsList.Count > 0)
                str += itemsList[0];
            for(int i = 1; i < itemsList.Count; i++)
                str += ", " + itemsList[i];
            str += "]";
            return str;
        }

    }

    class SignalInt:Signal
    {
        int v;
        public SignalInt(string s, ref int i)
        {
            v = 0;
            while(i < s.Length && s[i] >= '0' && s[i] <= '9')
            {
                v = 10*v + (s[i] - '0');
                i++;
            }
        }

        public override int CompareTo(Signal? other)
        {
            SignalInt? sig2 = other as SignalInt;
            if(sig2 == null){
                SignalArr thisArr = new SignalArr(this);
                return thisArr.CompareTo(other);
            }
            
            return v - sig2.v;
        }

        public override string ToString()
        {
            return v.ToString();
        }
    }

    private static void Main(string[] args)
    {
        List<KeyValuePair<Signal, int>> signalsList = new List<KeyValuePair<Signal, int>>();
        string[] lines = File.ReadAllLines("input.txt");
        int i, j;
        Signal sig;
        for(i = 0; i < lines.Length; i++)
        {
            j = 0;
            sig = Signal.CreateSignal(lines[i++], ref j);
            signalsList.Add(new KeyValuePair<Signal, int>(sig, i));
            
            j = 0;
            sig= Signal.CreateSignal(lines[i++], ref j);
            signalsList.Add(new KeyValuePair<Signal, int>(sig, i));
        }

        int id1 = i++;
        int id2 = i;

        j = 0;
        sig = Signal.CreateSignal("[[2]]", ref j);
        signalsList.Add(new KeyValuePair<Signal, int>(sig, id1));
        j = 0;
        sig = Signal.CreateSignal("[[6]]", ref j);
        signalsList.Add(new KeyValuePair<Signal, int>(sig, id2));

        signalsList.Sort((x, y) => x.Key.CompareTo(y.Key));

        int pos1 = -1;
        int pos2 = -1;
        i = 1;
        foreach(KeyValuePair<Signal, int> pair in signalsList)
        {
            if(pair.Value == id1){
                pos1 = i;
            }else if(pair.Value == id2){
                pos2 = i;
            }
            i++;
        }

        Console.WriteLine(pos1*pos2);
    }
}