namespace Runtime.Drinks {
    public interface IPourable {
        bool HasContent { get; }
        void EmptyContents();
        void GiveContent(IPourReceiver receiver);
    }
}