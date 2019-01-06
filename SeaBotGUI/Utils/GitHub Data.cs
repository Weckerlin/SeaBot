// SeabotGUI
// Copyright (C) 2018 - 2019 Weespin
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using J = Newtonsoft.Json.JsonPropertyAttribute;
using R = Newtonsoft.Json.Required;
using N = Newtonsoft.Json.NullValueHandling;

namespace SeaBotGUI.Utils
{
   
    class GitHub_Data
    {
        public class Root
        {
            [J("url")] public Uri Url { get; set; }
            [J("assets_url")] public Uri AssetsUrl { get; set; }
            [J("upload_url")] public string UploadUrl { get; set; }
            [J("html_url")] public Uri HtmlUrl { get; set; }
            [J("id")] public long Id { get; set; }
            [J("node_id")] public string NodeId { get; set; }
            [J("tag_name")] public string TagName { get; set; }
            [J("target_commitish")] public string TargetCommitish { get; set; }
            [J("name")] public string Name { get; set; }
            [J("draft")] public bool Draft { get; set; }
            [J("author")] public Author Author { get; set; }
            [J("prerelease")] public bool Prerelease { get; set; }
            [J("created_at")] public DateTimeOffset CreatedAt { get; set; }
            [J("published_at")] public DateTimeOffset PublishedAt { get; set; }
            [J("assets")] public List<Asset> Assets { get; set; }
            [J("tarball_url")] public Uri TarballUrl { get; set; }
            [J("zipball_url")] public Uri ZipballUrl { get; set; }
            [J("body")] public string Body { get; set; }
        }

        public class Asset
        {
            [J("url")] public Uri Url { get; set; }
            [J("id")] public long Id { get; set; }
            [J("node_id")] public string NodeId { get; set; }
            [J("name")] public string Name { get; set; }
            [J("label")] public object Label { get; set; }
            [J("uploader")] public Author Uploader { get; set; }
            [J("content_type")] public string ContentType { get; set; }
            [J("state")] public string State { get; set; }
            [J("size")] public long Size { get; set; }
            [J("download_count")] public long DownloadCount { get; set; }
            [J("created_at")] public DateTimeOffset CreatedAt { get; set; }
            [J("updated_at")] public DateTimeOffset UpdatedAt { get; set; }
            [J("browser_download_url")] public Uri BrowserDownloadUrl { get; set; }
        }

        public class Author
        {
            [J("login")] public string Login { get; set; }
            [J("id")] public long Id { get; set; }
            [J("node_id")] public string NodeId { get; set; }
            [J("avatar_url")] public Uri AvatarUrl { get; set; }
            [J("gravatar_id")] public string GravatarId { get; set; }
            [J("url")] public Uri Url { get; set; }
            [J("html_url")] public Uri HtmlUrl { get; set; }
            [J("followers_url")] public Uri FollowersUrl { get; set; }
            [J("following_url")] public string FollowingUrl { get; set; }
            [J("gists_url")] public string GistsUrl { get; set; }
            [J("starred_url")] public string StarredUrl { get; set; }
            [J("subscriptions_url")] public Uri SubscriptionsUrl { get; set; }
            [J("organizations_url")] public Uri OrganizationsUrl { get; set; }
            [J("repos_url")] public Uri ReposUrl { get; set; }
            [J("events_url")] public string EventsUrl { get; set; }
            [J("received_events_url")] public Uri ReceivedEventsUrl { get; set; }
            [J("type")] public string Type { get; set; }
            [J("site_admin")] public bool SiteAdmin { get; set; }
        }
    }
}