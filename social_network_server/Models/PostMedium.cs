﻿using System;
using System.Collections.Generic;

namespace social_network_server.Models;

public partial class PostMedium
{
    public int MediaId { get; set; }

    public int PostId { get; set; }

    public string MediaUrl { get; set; } = null!;

    public string MediaType { get; set; } = null!;

    public DateTime? CreateAt { get; set; }

    public virtual Post Post { get; set; } = null!;
}
