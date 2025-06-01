using System;
using System.Runtime.Serialization;

namespace DontPanic.TumblrSharp.Client
{
    /// <summary>
    /// type to filtered blog's activity feed
    /// </summary>
    [Flags]
    public enum NotificationsTypes
    {
        /// <summary>
        /// a like your post
        /// </summary>
        [EnumMember(Value = "like")]
        Like = 1 << 0,

        /// <summary>
        /// a reply on your post
        /// </summary>
        [EnumMember(Value = "reply")]
        Reply = 1 << 1,

        /// <summary>
        /// a new follower
        /// </summary>
        [EnumMember(Value = "follower")]
        Follower = 1 << 2,

        /// <summary>
        /// A mention of your blog in a reply
        /// </summary>
        [EnumMember(Value = "mention_in_reply")]
        MentionInReply = 1 << 3,

        /// <summary>
        /// A mention of your blog in a post
        /// </summary>
        [EnumMember(Value = "mention_in_post")]
        MentionInPost = 1 << 4,

        /// <summary>
        /// A reblog of your post, without commentary
        /// </summary>
        [EnumMember(Value = "reblog_naked")]
        ReblogNaked = 1 << 5,

        /// <summary>
        /// A reblog of your post, with commentary
        /// </summary>
        [EnumMember(Value = "reblog_with_content")]
        ReblogWithContent = 1 << 6,

        /// <summary>
        /// A new ask received
        /// </summary>
        [EnumMember(Value = "ask")]
        Ask = 1 << 7,

        /// <summary>
        /// An answered ask that you had sent
        /// </summary>
        [EnumMember(Value = "answered_ask")]
        AnsweredAsk = 1 << 8,
                
        /// <summary>
        /// A new member of your group blog
        /// </summary>
        [EnumMember(Value = "new_group_blog_member")]
        NewGroupBlogMember = 1 << 9,

        /// <summary>
        /// Someone using your post content in their post
        /// </summary>
        [EnumMember(Value = "post_attribution")]
        PostAttribution = 1 << 10,

        /// <summary>
        /// A post of yours being flagged
        /// </summary>
        [EnumMember(Value = "post_flagged")]
        PostFlagged = 1 << 11,

        /// <summary>
        /// An appeal accepted
        /// </summary>
        [EnumMember(Value = "post_appeal_accepted")]
        PostAppealAccepted = 1 << 12,

        /// <summary>
        /// An appeal rejected
        /// </summary>
        [EnumMember(Value = "post_appeal_rejected")]
        PostAppealRejected = 1 << 13,

        /// <summary>
        /// A post we think you missed
        /// </summary>
        [EnumMember(Value = "what_you_missed")]
        WhatYouMissed = 1 << 14,

        /// <summary>
        /// A conversational note (reply, reblog with comment) on a post you're watching
        /// </summary>
        [EnumMember(Value = "conversational_note")]
        ConversationalNote = 1 << 15,


        /// <summary>
        /// Milestone Birthday
        /// </summary>
        [EnumMember(Value = "milestone_birthday")]
        MilestoneBirthday = 1 << 16,

        /// <summary>
        /// all NotificationsTypes
        /// </summary>
        All = Like | Reply | Follower | MentionInReply | MentionInPost | MentionInPost |
            ReblogNaked | ReblogWithContent | Ask | AnsweredAsk | NewGroupBlogMember |
            PostAttribution | PostFlagged | PostAppealAccepted | PostAppealRejected |
            WhatYouMissed | ConversationalNote | MilestoneBirthday
    }
}
