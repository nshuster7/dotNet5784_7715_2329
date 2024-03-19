using BO;


namespace BlApi
{
    public interface IUser
    {
        /// <summary>
        /// Gets an user and adds it to the list of users
        /// </summary>
        /// <param name="user"></param>
        public void Create(User user);

        /// <summary>
        /// Return a list of the users
        /// </summary>
        /// <returns>IEnumerable<User?></returns>
        public IEnumerable<User?> ReadAll(Func<DO.User, bool>? filter = null);

        /// <summary>
        /// Gets user's ID and returns user with this ID
        /// </summary>
        /// <param name="userID"></param>
        /// <returns>User</returns>
        public User Read(int userID);

        /// <summary>
        /// Gets user's ID and password and resets his password
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="password"></param>
        public void ResetPassword(int ID, string password);
        public DO.User? GetByUserName(string userName);
    }
}
