using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using MutationTestingMeetup.Domain;
using Newtonsoft.Json;

namespace MutationTestingMeetup.Tests.Asserters
{
    public class HttpResponseMessageAsserter : AbstractAsserter<HttpResponseMessage, HttpResponseMessageAsserter>
    {

        public static HttpResponseMessageAsserter AssertThat(HttpResponseMessage httpResponseMessage)
        {
            return new HttpResponseMessageAsserter(httpResponseMessage);
        }

        public HttpResponseMessageAsserter(HttpResponseMessage actual) : base(actual)
        {
        }

        public HttpResponseMessageAsserter HasFailedStatus()
        {
            Actual.IsSuccessStatusCode.Should().BeFalse();

            return this;
        }

        public async Task<HttpResponseMessageAsserter> HasStatusCode(HttpStatusCode statusCode)
        {
            var bodyContent = await Actual.Content.ReadAsStringAsync();
            Actual.StatusCode.Should().Be(statusCode, bodyContent);

            return this;
        }

        public async Task<HttpResponseMessageAsserter> HasEmptyJsonBody()
        {
            var content = await Actual.Content.ReadAsStringAsync();
            content.Should().BeEmpty();

            return this;
        }

        public async Task<HttpResponseMessageAsserter> HasJsonInBody(string expectedJson)
        {
            var content = await Actual.Content.ReadAsStringAsync();
            JsonAsserter.AssertThat(content)
                .IsEqualTo(expectedJson);

            return this;
        }

        public async Task<HttpResponseMessageAsserter> HasTextInBody(string expectedText)
        {
            var content = await Actual.Content.ReadAsStringAsync();

            content.Should().Be(expectedText);
            
            return this;
        }

        public async Task<HttpResponseMessageAsserter> HasJsonInBody(dynamic expectedJson)
        {
            JsonAsserter.AssertThat(await Actual.Content.ReadAsStringAsync())
                .IsEqualTo(JsonConvert.SerializeObject(expectedJson));

            return this;
        }

        public async Task<HttpResponseMessageAsserter> HasJsonArrayInBody(string expectedJson)
        {
            JsonAsserter.AssertThat(await Actual.Content.ReadAsStringAsync())
                .IsEqualToArray(expectedJson);

            return this;
        }

        public async Task<HttpResponseMessageAsserter> HasJsonArrayInBody(dynamic expectedJson)
        {
            JsonAsserter.AssertThat(await Actual.Content.ReadAsStringAsync())
                .IsEqualToArray(JsonConvert.SerializeObject(expectedJson));

            return this;
        }

        public async Task<HttpResponseMessageAsserter> HasJsonArrayInBody<TAsserter>(params Func<TAsserter>[] entityAsserters) where TAsserter : AbstractAsserter<Product, TAsserter>
        {
            foreach (var asserter in entityAsserters)
            {
                asserter.Invoke();
            }

            return this;
        }

        public async Task<HttpResponseMessageAsserter> HasEmptyJsonArrayInBody()
        {
            JsonAsserter.AssertThat(await Actual.Content.ReadAsStringAsync())
                .IsEqualToArray(JsonConvert.SerializeObject(new dynamic[] { }));

            return this;
        }
    }
}
