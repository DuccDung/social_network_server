﻿using System;
using System.Collections.Generic;

namespace social_network_server.Models;

public partial class Post
{
    public int PostId { get; set; }

    public int AccountId { get; set; }

    public string? Content { get; set; }

    public string? PostType { get; set; }

    public DateTime? CreateAt { get; set; }

    public DateTime? UpdateAt { get; set; }

    public bool? IsRemove { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual ICollection<PostComment> PostComments { get; set; } = new List<PostComment>();

    public virtual ICollection<PostMedium> PostMedia { get; set; } = new List<PostMedium>();

    public virtual ICollection<PostShare> PostShares { get; set; } = new List<PostShare>();
}
