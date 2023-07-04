using DJImageAuthorizationServer.Entities;
using Fido2NetLib;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace DJImageAuthorizationServer.Fido2;

public class Fido2Store
{
    private readonly ApplicationDbContext applicationDbContext;

    public Fido2Store(ApplicationDbContext applicationDbContext)
    {
        this.applicationDbContext = applicationDbContext;
    }

    public async Task<ICollection<FidoStoredCredential>> GetCredentialsByUserNameAsync(string username)
    {
        return await applicationDbContext.FidoStoredCredential.Where(c => c.UserName == username).ToListAsync();
    }

    public async Task RemoveCredentialsByUserNameAsync(string username)
    {
        var items = await applicationDbContext.FidoStoredCredential.Where(c => c.UserName == username).ToListAsync();
        if (items != null)
        {
            foreach (var fido2Key in items)
            {
                applicationDbContext.FidoStoredCredential.Remove(fido2Key);
            };

            await applicationDbContext.SaveChangesAsync();
        }
    }

    public async Task<FidoStoredCredential?> GetCredentialByIdAsync(byte[] id)
    {
        var credentialIdString = Base64Url.Encode(id);
        //byte[] credentialIdStringByte = Base64Url.Decode(credentialIdString);

        var cred = await applicationDbContext.FidoStoredCredential
            .Where(c => c.DescriptorJson != null && c.DescriptorJson.Contains(credentialIdString))
            .FirstOrDefaultAsync();

        return cred;
    }

    public Task<ICollection<FidoStoredCredential>> GetCredentialsByUserHandleAsync(byte[] userHandle)
    {
        return Task.FromResult<ICollection<FidoStoredCredential>>(
            applicationDbContext
                .FidoStoredCredential.Where(c => c.UserHandle != null && c.UserHandle.SequenceEqual(userHandle))
                .ToList());
    }

    public async Task UpdateCounterAsync(byte[] credentialId, uint counter)
    {
        var credentialIdString = Base64Url.Encode(credentialId);
        //byte[] credentialIdStringByte = Base64Url.Decode(credentialIdString);

        var cred = await applicationDbContext.FidoStoredCredential
            .Where(c => c.DescriptorJson != null && c.DescriptorJson.Contains(credentialIdString)).FirstOrDefaultAsync();

        if (cred != null)
        {
            cred.SignatureCounter = counter;
            await applicationDbContext.SaveChangesAsync();
        }
    }

    public async Task AddCredentialToUserAsync(Fido2User user, FidoStoredCredential credential)
    {
        credential.UserId = user.Id;
        applicationDbContext.FidoStoredCredential.Add(credential);
        await applicationDbContext.SaveChangesAsync();
    }

    public async Task<ICollection<Fido2User>> GetUsersByCredentialIdAsync(byte[] credentialId)
    {
        var credentialIdString = Base64Url.Encode(credentialId);
        //byte[] credentialIdStringByte = Base64Url.Decode(credentialIdString);

        var cred = await applicationDbContext.FidoStoredCredential
            .Where(c => c.DescriptorJson != null && c.DescriptorJson.Contains(credentialIdString)).FirstOrDefaultAsync();

        if (cred == null || cred.UserId == null)
        {
            return new List<Fido2User>();
        }

        return await applicationDbContext.Users
                .Where(u => Encoding.UTF8.GetBytes(u.UserName)
                .SequenceEqual(cred.UserId))
                .Select(u => new Fido2User
                {
                    DisplayName = u.UserName,
                    Name = u.UserName,
                    Id = Encoding.UTF8.GetBytes(u.UserName) // byte representation of userID is required
                }).ToListAsync();
    }
}

public static class Fido2Extenstions
{
    public static IEnumerable<T> NotNull<T>(this IEnumerable<T?> enumerable) where T : class
    {
        return enumerable.Where(e => e != null).Select(e => e!);
    }
}