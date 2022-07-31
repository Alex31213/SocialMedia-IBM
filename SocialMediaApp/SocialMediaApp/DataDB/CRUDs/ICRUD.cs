namespace SocialMediaApp.DataDB.CRUDs
{
    public interface ICRUD<T> where T : class
    {
        public void Add(T Added);
        public T GetByID(int ID);
        public void UpdateByID(int ID, T Updated);
        public void RemoveByID(int ID);
    }
}
