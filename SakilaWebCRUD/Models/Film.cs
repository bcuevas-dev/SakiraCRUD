﻿using System;
using System.Collections.Generic;

namespace SakilaWebCRUD.Models;

public partial class Film
{
    public ushort FilmId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public short? ReleaseYear { get; set; }

    public byte LanguageId { get; set; }

    public byte? OriginalLanguageId { get; set; }

    public byte RentalDuration { get; set; }

    public decimal RentalRate { get; set; }

    public ushort? Length { get; set; }

    public decimal ReplacementCost { get; set; }

    public string? Rating { get; set; }

    public DateTime LastUpdate { get; set; }

    public virtual ICollection<FilmActor> FilmActors { get; set; } = new List<FilmActor>();

    public virtual ICollection<FilmCategory> FilmCategories { get; set; } = new List<FilmCategory>();

    public virtual ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();

    public virtual Language Language { get; set; } = null!;

    public virtual Language? OriginalLanguage { get; set; }
}
