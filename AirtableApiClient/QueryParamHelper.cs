﻿using System.Collections.Generic;
using System.Web;
using System.Text.Json.Serialization;

namespace AirtableApiClient
{
    public enum SortDirection
    {
        Asc,
        Desc
    }


    public class Sort
    {
        [JsonPropertyName("fields")]
        public string Field { get; set; }

        [JsonPropertyName("direction")]
        public SortDirection Direction { get; set; }
    }


    public class Fields
    {
        [JsonPropertyName("fields")]
        public IDictionary<string, object> FieldsCollection { get; set; } = new Dictionary<string, object>();

        public void AddField(string fieldName, object fieldValue)
        {
            FieldsCollection.Add(fieldName, fieldValue);
        }
    }


    /// <summary>
    /// This class is used in the Update and Replace record(s) operations.
    /// </summary>
    public class IdFields : Fields
    {
        public IdFields(string id)
        {
            this.id = id;
        }

        // Note: System.Text.Json's Serialization includes Properties by default
        // So it's good practice to use Property instead of Fields.
        // Change 'field' to 'property' but not changing the case of 'i' in 'Id'
        // to keep backward compatiblity.
        [JsonPropertyName("id")]
        public string id { get; set; }      // Note: this is intended to be the record ID of the record containing Fields (not to be confused with the field ID)
    }


    internal class QueryParamHelper
    {
        static internal string
        FlattenSortParam(
            IEnumerable<Sort> sort)
        {
            int i = 0;
            string flattenSortParam = string.Empty;
            string toInsert = string.Empty;
            foreach (var sortItem in sort)
            {
                if (string.IsNullOrEmpty(toInsert) && i > 0)
                {
                    toInsert = "&";
                }

                // Name of fields to be sorted
                string param = $"sort[{i}][field]";
                flattenSortParam += $"{toInsert}{HttpUtility.UrlEncode(param)}={HttpUtility.UrlEncode(sortItem.Field)}";

                // Direction for sorting
                param = $"sort[{i}][direction]";
                flattenSortParam += $"&{HttpUtility.UrlEncode(param)}={HttpUtility.UrlEncode(sortItem.Direction.ToString().ToLower())}";
                i++;
            }
            return flattenSortParam;
        }


        static internal string 
        FlattenFieldsParam(
            IEnumerable<string> fields)
        {
            int i = 0;
            string flattenFieldsParam = string.Empty;
            string toInsert = string.Empty;
            foreach (var fieldName in fields)
            {
                if (string.IsNullOrEmpty(toInsert) && i > 0)
                {
                    toInsert = "&";
                }
                string param = "fields[]";
                flattenFieldsParam += $"{toInsert}{HttpUtility.UrlEncode(param)}={HttpUtility.UrlEncode(fieldName)}";
                i++;
            }
            return flattenFieldsParam;
        }

    }   // end class
}   // end namespace 
