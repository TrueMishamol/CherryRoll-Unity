using System.Collections.Generic;
using Unity.Netcode;

[System.Serializable]
public class SerializableDictionary : INetworkSerializable {


    public Dictionary<ulong, int> dictionary = new Dictionary<ulong, int>();


    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter {
        if (serializer.IsReader) {
            int count = 0;
            serializer.SerializeValue(ref count);

            for (int i = 0; i < count; i++) {
                ulong key = 0;
                int value = 0;
                serializer.SerializeValue(ref key);
                serializer.SerializeValue(ref value);
                dictionary[key] = value;
            }
        } else if (serializer.IsWriter) {
            int count = dictionary.Count;
            serializer.SerializeValue(ref count);

            foreach (var pair in dictionary) {
                ulong key = pair.Key;
                int value = pair.Value;
                serializer.SerializeValue(ref key);
                serializer.SerializeValue(ref value);
            }
        }
    }
}
