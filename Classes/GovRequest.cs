using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIGovApp.Classes
{
    public class GovRequest
    {
        public class Dummy
        {}

        public class Facets
        {
            public Results results { get; set; }
        }

        public class Organization
        {
            public string description { get; set; }
            public string created { get; set; }
            public string title { get; set; }
            public string name { get; set; }
            public bool is_organization { get; set; }
            public string state { get; set; }
            public string image_url { get; set; }
            public string revision_id { get; set; }
            public string type { get; set; }
            public string id { get; set; }
            public string approval_status { get; set; }
        }

        public class Request
        {
            public Facets facets { get; set; }
            public Organization organization { get; set; }
            public Resource res { get; set; }
            public Result result { get; set; }
            public Result2 result2 { get; set; }
            public Results results { get; set; }
            public Results2 results2 { get; set; }
            public RootObject root_obj { get; set; }
            public SearchFacets search_facets { get; set; }
            public Tag tag { get; set; }
        }

        public class Resource
        {
            public String cache_last_updated { get; set; }
            public String package_id { get; set; }
            public String webstore_last_updated { get; set; }
            public bool datastore_active { get; set; }
            public String id { get; set; }
            public String size { get; set; }
            public String romania_download_url { get; set; }
            public String state { get; set; }
            public String hash { get; set; }
            public String description { get; set; }
            public String format { get; set; }
            public object mimetype_inner { get; set; }
            public String url_type { get; set; }
            public String mimetype { get; set; }
            public String cache_url { get; set; }
            public string name { get; set; }
            public string created { get; set; }
            public String url { get; set; }
            public String webstore_url { get; set; }
            public String last_modified { get; set; }
            public int position { get; set; }
            public String revision_id { get; set; }
            public String resource_type { get; set; }
        }

        public class Result
        {
            public int count { get; set; }
            public string sort { get; set; }
            public Facets facets { get; set; }
            public List<Result2> results { get; set; }
            public SearchFacets search_facets { get; set; }
        }

        public class Result2
        {
            public String license_title { get; set; }
            public String maintainer { get; set; }
            public List<Dummy> relationships_as_object { get; set; }
            public bool @private { get; set; }
            public String maintainer_email { get; set; }
            public int num_tags { get; set; }
            public String id { get; set; }
            public String metadata_created { get; set; }
            public String metadata_modified { get; set; }
            public String author { get; set; }
            public String author_email { get; set; }
            public String state { get; set; }
            public String version { get; set; }
            public String creator_user_id { get; set; }
            public String type { get; set; }
            public List<Resource> resources { get; set; }
            public int num_resources { get; set; }
            public List<Tag> tags { get; set; }
            public List<Dummy> groups { get; set; }
            public String license_id { get; set; }
            public List<Dummy> relationships_as_subject { get; set; }
            public Organization organization { get; set; }
            public String name { get; set; }
            public bool isopen { get; set; }
            public String url { get; set; }
            public String notes { get; set; }
            public String owner_org { get; set; }
            public List<Dummy> extras { get; set; }
            public String license_url { get; set; }
            public String title { get; set; }
            public String revision_id { get; set; }
        }

        public class Results
        {}

        public class Results2
        {
            public List<Dummy> items { get; set; }
            public string title { get; set; }
        }

        public class RootObject
        {
            public string help { get; set; }
            public bool success { get; set; }
            public Result result { get; set; }
        }

        public class SearchFacets
        {
            public Results2 results { get; set; }
        }

        public class Tag
        {
            public Dummy vocabulary_id { get; set; }
            public string state { get; set; }
            public string display_name { get; set; }
            public string id { get; set; }
            public string name { get; set; }
        }
    }
}