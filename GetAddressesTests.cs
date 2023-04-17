﻿//-----------------------------------------------------------------------
// <copyright file="GetAddressesTests.cs" company="Procare Software, LLC">
//     Copyright © 2021-2023 Procare Software, LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Procare.Address.IntegrationTests;

using System;
using System.Buffers.Text;
using System.Threading.Tasks;

using Xunit; // built in testing library
using static Procare.Address.IntegrationTests.GetAddressesResponse;
using System.Net.Mail;

public class GetAddressesTests
{
    private readonly AddressService service = new AddressService(new Uri("https://address.dev-procarepay.com"));

    [Fact] 
    public async Task GetAddresses_With_Owm_ShouldResultIn_OneMatchingAddress()
    {
        // Sends request to API with provided data
        var result = await this.service.GetAddressesAsync(new AddressFilter { Line1 = "1 W Main St", City = "Medford", StateCode = "OR" }).ConfigureAwait(false);

        Assert.NotNull(result); // checks that entire result is not null
        Assert.Equal(1, result.Count); // checks that Addresses returned = 1
        Assert.NotNull(result.Addresses); // checks if no addresses found - valid responses being null or a list (obj, arr, etc)
        Assert.Equal(result.Count, result.Addresses!.Count); // ! > null forgiving operator, Business Logic Test
    }

    [Fact]
    public async Task GetAddresses_With_AmbiguousAddress_ShouldResultIn_MultipleMatchingAddresses()
    {
        var result = await this.service.GetAddressesAsync(new AddressFilter { Line1 = "123 Main St", City = "Ontario", StateCode = "CA" }).ConfigureAwait(false);

        // Including the following three tests because they are also applicable to this test case, may be redundant
        Assert.NotNull(result);
        Assert.NotNull(result.Addresses);
        Assert.Equal(result.Count, result.Addresses!.Count);

        // Count not equal to 0 or 1 (expect multiple responses)
        Assert.True(result.Count > 1);
        Assert.True(result.Addresses.Count > 1);

        // throw new NotImplementedException();
    }

    [Fact]
    public async Task GetAddresses_With_UKAddress_ShouldResultIn_NoAddressesReturned() // Expect no addresses returned
    {
        var result = await this.service.GetAddressesAsync(new AddressFilter { Line1 = "21 New Globe Walk", City = "London" }).ConfigureAwait(false);

        // Including the following three tests because they are also applicable to this test case, may be redundant
        Assert.NotNull(result);
        Assert.NotNull(result.Addresses);
        Assert.Equal(result.Count, result.Addresses!.Count);
        // Check that no Addresses were returned
        Assert.Equal(0, result.Count);
    }

    //[Fact]
    //public async Task GetAddresses_With_AmbiguousAddress_NoCityProvided_ShouldResultIn_NullResponse()
    //{
    //    var result = await this.service.GetAddressesAsync(new AddressFilter { Line1 = "123 Main St", StateCode = "CA" }).ConfigureAwait(false);

    //    Assert.Null(result);
    //    Assert.Equal(System.Net.HttpStatusCode.BadRequest, System.Net.Http.HttpRequestException);
    //}

    // Both of these tests returned 400, so starting with the assumption that this is the expected behavior &
    // will confirm this expectation with team
    [Fact]
    public async Task GetAddresses_With_EmptyRequest_ShouldResultIn_ExceptionBeingThrown()
    {
        var result = await this.service.GetAddressesAsync(new AddressFilter { }).ConfigureAwait(false);

        //var response = await Assert.ThrowsAsync<System.Net.HttpStatusCode.BadRequest>(async () => result)

        // var ex = await Assert.ThrowsAsync<HttpException>(async () => await emailController.AppendEmailBase64Dto(testEmailBase64Dto));

        //throw new NotImplementedException();
    }

    //[Fact]
    //public async Task GetAddresses_With_EmptyRequestStrings_ShouldResultIn_ExceptionBeingThrown()
    //{
    //    var filter = new AddressFilter {
    //        Line1 = string.Empty,
    //        City = string.Empty,
    //        StateCode = string.Empty,
    //        CompanyName = string.Empty,
    //        Line2 = string.Empty,
    //        Urbanization = string.Empty,
    //        ZipCodeLeading5 = string.Empty,
    //        ZipCodeTrailing4 = string.Empty
    //    };
    //    var result = await this.service.GetAddressesAsync(filter).ConfigureAwait(false);


    //}
}


