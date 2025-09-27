﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Semi.Avalonia.Demo.ViewModels;

public partial class IconDemoViewModel : ObservableObject
{
    private readonly Icons _resources = new();

    private readonly Dictionary<string, IconItem> _filledIcons = new();
    private readonly Dictionary<string, IconItem> _strokedIcons = new();

    [ObservableProperty] private string? _searchText;

    public ObservableCollection<IconItem> FilteredFilledIcons { get; set; } = [];
    public ObservableCollection<IconItem> FilteredStrokedIcons { get; set; } = [];

    public void InitializeResources()
    {
        foreach (var provider in _resources.MergedDictionaries)
        {
            if (provider is not ResourceDictionary dic) continue;

            foreach (var key in dic.Keys)
            {
                if (dic[key] is not Geometry geometry) continue;
                var resourceKey = key.ToString() ?? string.Empty;
                var icon = new IconItem
                {
                    ResourceKey = resourceKey,
                    Geometry = geometry
                };

                if (resourceKey.EndsWith("Stroked", StringComparison.InvariantCultureIgnoreCase))
                    _strokedIcons[resourceKey] = icon;
                else
                    _filledIcons[resourceKey] = icon;
            }
        }

        OnSearchTextChanged(string.Empty);
    }

    partial void OnSearchTextChanged(string? value)
    {
        var search = string.IsNullOrWhiteSpace(value) ? string.Empty : value.Trim();

        FilteredFilledIcons.Clear();
        foreach (var pair in _filledIcons.Where(kv => kv.Key.Contains(search, StringComparison.InvariantCultureIgnoreCase)))
        {
            FilteredFilledIcons.Add(pair.Value);
        }

        FilteredStrokedIcons.Clear();
        foreach (var pair in _strokedIcons.Where(kv => kv.Key.Contains(search, StringComparison.InvariantCultureIgnoreCase)))
        {
            FilteredStrokedIcons.Add(pair.Value);
        }
    }
}

public class IconItem
{
    public string? ResourceKey { get; set; }
    public Geometry? Geometry { get; set; }
}