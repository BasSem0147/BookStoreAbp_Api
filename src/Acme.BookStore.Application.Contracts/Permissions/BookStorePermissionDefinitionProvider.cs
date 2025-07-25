using Acme.BookStore.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace Acme.BookStore.Permissions;

public class BookStorePermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(BookStorePermissions.GroupName);

        //Define your own permissions here. Example:
        //myGroup.AddPermission(BookStorePermissions.MyPermission1, L("Permission:MyPermission1"));
        var AuthorGroup = context.AddGroup(BookStorePermissions.AuthorGroupName, L("Demo.Authors"));
        AuthorGroup.AddPermission(BookStorePermissions.CreateEditAuthorPermission, L("Permission:Author:CreateEditAuthor"));
        AuthorGroup.AddPermission(BookStorePermissions.DeleteAuthorPermission, L("Permission:Author:DeleteAuthor"));
        AuthorGroup.AddPermission(BookStorePermissions.GetAuthorPermission, L("Permission:Author:GetAuthor"));
        AuthorGroup.AddPermission(BookStorePermissions.ListAuthorPermission, L("Permission:Author:ListAuthor"));

        var BookGroup = context.AddGroup(BookStorePermissions.BookGroupName, L("Demo.Books"));
        BookGroup.AddPermission(BookStorePermissions.CreateEditBookPermission, L("Permission:Book:CreateEditBook"));
        BookGroup.AddPermission(BookStorePermissions.DeleteBookPermission, L("Permission:Book:DeleteBook"));
        BookGroup.AddPermission(BookStorePermissions.GetBookPermission, L("Permission:Book:GetBook"));
        BookGroup.AddPermission(BookStorePermissions.ListBookPermission, L("Permission:Book:ListBook"));


    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<BookStoreResource>(name);
    }
}
