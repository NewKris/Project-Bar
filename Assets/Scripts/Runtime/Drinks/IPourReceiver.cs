namespace Runtime.Drinks {
    public interface IPourReceiver {
        public void AddContents(DrinkContents contents);
        public void AddContents(IngredientGroup group);
    }
}