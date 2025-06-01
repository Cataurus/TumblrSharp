using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;

namespace DontPanic.TumblrSharp.Client
{
    /// <summary>
    /// Extension for enum NotificationTypes
    /// </summary>
    internal static class ExtensionNotification
    {

        private static string ToTumblrString(this NotificationsTypes notificationsTypes)
        {
            string result;

            FieldInfo fieldInfo = notificationsTypes.GetType().GetField(notificationsTypes.ToString());

            EnumMemberAttribute jsonPropertyAttribute = (EnumMemberAttribute)Attribute.GetCustomAttribute(fieldInfo, typeof(EnumMemberAttribute));

            result = jsonPropertyAttribute.Value;

            return result;
        }

        /// <summary>
        /// Converts to the corresponding string array for tumblr
        /// </summary>
        /// <param name="notificationsTypes"></param>
        /// <returns>array of tumblr strings</returns>
        public static string[] ToTumblrStringArray(this NotificationsTypes notificationsTypes)
        {
            List<string> result = new List<string>();

            if (notificationsTypes.HasFlag(NotificationsTypes.AnsweredAsk))
                result.Add(NotificationsTypes.AnsweredAsk.ToTumblrString());

            if (notificationsTypes.HasFlag(NotificationsTypes.Ask))
                result.Add(NotificationsTypes.Ask.ToTumblrString());

            if (notificationsTypes.HasFlag(NotificationsTypes.ConversationalNote))
                result.Add(NotificationsTypes.ConversationalNote.ToTumblrString());

            if (notificationsTypes.HasFlag(NotificationsTypes.Follower))
                result.Add(NotificationsTypes.Follower.ToTumblrString());

            if (notificationsTypes.HasFlag(NotificationsTypes.Like))
                result.Add(NotificationsTypes.Like.ToTumblrString());

            if (notificationsTypes.HasFlag(NotificationsTypes.MentionInPost))
                result.Add(NotificationsTypes.MentionInPost.ToTumblrString());

            if (notificationsTypes.HasFlag(NotificationsTypes.MentionInReply))
                result.Add(NotificationsTypes.MentionInReply.ToTumblrString());

            if (notificationsTypes.HasFlag(NotificationsTypes.NewGroupBlogMember))
                result.Add(NotificationsTypes.NewGroupBlogMember.ToTumblrString());

            if (notificationsTypes.HasFlag(NotificationsTypes.PostAppealAccepted))
                result.Add(NotificationsTypes.PostAppealAccepted.ToTumblrString());

            if (notificationsTypes.HasFlag(NotificationsTypes.PostAppealRejected))
                result.Add(NotificationsTypes.PostAppealRejected.ToTumblrString());

            if (notificationsTypes.HasFlag(NotificationsTypes.PostAttribution))
                result.Add(NotificationsTypes.PostAttribution.ToTumblrString());

            if (notificationsTypes.HasFlag(NotificationsTypes.PostFlagged))
                result.Add(NotificationsTypes.PostFlagged.ToTumblrString());

            if (notificationsTypes.HasFlag(NotificationsTypes.ReblogNaked))
                result.Add(NotificationsTypes.ReblogNaked.ToTumblrString());

            if (notificationsTypes.HasFlag(NotificationsTypes.ReblogWithContent))
                result.Add(NotificationsTypes.ReblogWithContent.ToTumblrString());

            if (notificationsTypes.HasFlag(NotificationsTypes.Reply))
                result.Add(NotificationsTypes.Reply.ToTumblrString());

            if (notificationsTypes.HasFlag(NotificationsTypes.WhatYouMissed))
                result.Add(NotificationsTypes.WhatYouMissed.ToTumblrString());

            if (notificationsTypes.HasFlag(NotificationsTypes.MilestoneBirthday))
                result.Add(NotificationsTypes.MilestoneBirthday.ToTumblrString());

            return result.ToArray();
        }

    }
}
