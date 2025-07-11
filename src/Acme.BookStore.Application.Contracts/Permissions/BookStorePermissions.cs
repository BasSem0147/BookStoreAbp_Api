namespace Acme.BookStore.Permissions;

public static class BookStorePermissions
{
    public const string GroupName = "BookStore";



    public const string AuthorGroupName = GroupName + ".Authors";
    public const string CreateEditAuthorPermission = AuthorGroupName + ".CreateEdit";
    public const string DeleteAuthorPermission = AuthorGroupName + ".Delete";
    public const string GetAuthorPermission = AuthorGroupName + ".Get";
    public const string ListAuthorPermission = AuthorGroupName + ".List";
}
