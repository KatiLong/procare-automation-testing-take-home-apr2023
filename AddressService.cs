//-----------------------------------------------------------------------
// <copyright file="AddressService.cs" company="Procare Software, LLC">
//     Copyright © 2021-2023 Procare Software, LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Procare.Address.IntegrationTests;

using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

public class AddressService
{
    private static readonly HttpClient HttpClient = new HttpClient();

    public AddressService(Uri baseUrl)
    {
        this.BaseUrl = baseUrl;
    }

    public Uri BaseUrl { get; }

    public async Task<GetAddressesResponse> GetAddressesAsync(AddressFilter request, CancellationToken token = default)
    {
        using (var httpRequest = request.ToHttpRequest(this.BaseUrl))
        using (var response = await HttpClient.SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead, token).ConfigureAwait(false))
        {
            
            //var data = db.GetThisAddress(requestData); 
            //var response = new GetAddressesResponse(); //empty object
            //response.Addresses = data.Addresses; //set the object using what we got from the database
            //response.Count = data.Addresses.Count;
            //return response;

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync(token).ConfigureAwait(false);

            return JsonSerializer.Deserialize<GetAddressesResponse>(content) !;
        }
    }
}
