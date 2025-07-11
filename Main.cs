// Copyright (c) Microsoft Corporation
// The Microsoft Corporation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using Community.PowerToys.Run.Plugin.UUIDConverter.Properties;
using ManagedCommon;
using Microsoft.PowerToys.Settings.UI.Library;
using Wox.Infrastructure;
using Wox.Plugin;
using Wox.Plugin.Logger;

namespace Community.PowerToys.Run.Plugin.UUIDConverter
{
    public class Main : IPlugin, IPluginI18n, IContextMenu, ISettingProvider, IReloadable, IDisposable, IDelayedExecutionPlugin
    {
        private const string Setting = nameof(Setting);

        // current value of the setting
        private bool _setting;
        private PluginInitContext? _context;
        private string? _iconPath;
        private bool _disposed;

        public string Name => Resources.plugin_name;

        public string Description => Resources.plugin_description;

        // TODO: remove dash from ID below and inside plugin.json
        public static string PluginID => "c27733f390ae4056b6e1b012f63045b7";

        // TODO: add additional options (optional)
        public IEnumerable<PluginAdditionalOption> AdditionalOptions => new List<PluginAdditionalOption>()
        {
            new PluginAdditionalOption()
            {
                PluginOptionType = PluginAdditionalOption.AdditionalOptionType.Checkbox,
                Key = Setting,
                DisplayLabel = Resources.plugin_setting,
            },
        };

        public void UpdateSettings(PowerLauncherPluginSettings settings)
        {
            _setting = settings?.AdditionalOptions?.FirstOrDefault(x => x.Key == Setting)?.Value ?? false;
        }

        // TODO: return context menus for each Result (optional)
        public List<ContextMenuResult> LoadContextMenus(Result selectedResult)
        {
            return new List<ContextMenuResult>(0);
        }

        // TODO: return query results
        public List<Result> Query(Query query)
        {
            ArgumentNullException.ThrowIfNull(query);

            var results = new List<Result>();

            // empty query
            if (string.IsNullOrEmpty(query.Search))
            {
                results.Add(new Result
                {
                    Title = Name,
                    SubTitle = Description,
                    QueryTextDisplay = string.Empty,
                    IcoPath = _iconPath,
                    Action = action =>
                    {
                        return true;
                    },
                });
                return results;
            }

            if (query.Search.Contains('-'))
            {
                string new_uuid = query.Search.Replace("-", string.Empty);
                results.Add(new Result
                {
                    Title = new_uuid,
                    SubTitle = Description,
                    QueryTextDisplay = new_uuid,
                    IcoPath = _iconPath,
                    Action = action =>
                    {
                        Clipboard.SetText(new_uuid);
                        return true;
                    },
                });
            }
            else
            {
                string input = query.Search;
                if (input.Length != 32)
                {
                    throw new ArgumentException("Input UUID must be 32 characters long (without hyphens).");
                }

                string new_uuid = string.Format(CultureInfo.InvariantCulture, "{0}-{1}-{2}-{3}-{4}", input.Substring(0, 8), input.Substring(8, 4), input.Substring(12, 4), input.Substring(16, 4), input.Substring(20, 12));

                results.Add(new Result
                {
                    Title = new_uuid,
                    SubTitle = Description,
                    QueryTextDisplay = new_uuid,
                    IcoPath = _iconPath,
                    Action = action =>
                    {
                        Clipboard.SetText(new_uuid);
                        return true;
                    },
                });
            }

            return results;
        }

        // TODO: return delayed query results (optional)
        public List<Result> Query(Query query, bool delayedExecution)
        {
            ArgumentNullException.ThrowIfNull(query);

            var results = new List<Result>();

            // empty query
            if (string.IsNullOrEmpty(query.Search))
            {
                results.Add(new Result
                {
                    Title = Name,
                    SubTitle = Description,
                    QueryTextDisplay = string.Empty,
                    IcoPath = _iconPath,
                    Action = action =>
                    {
                        return true;
                    },
                });
                return results;
            }

            if (query.Search.Contains('-'))
            {
                string new_uuid = query.Search.Replace("-", string.Empty);
                results.Add(new Result
                {
                    Title = new_uuid,
                    SubTitle = Description,
                    QueryTextDisplay = new_uuid,
                    IcoPath = _iconPath,
                    Action = action =>
                    {
                        Clipboard.SetText(new_uuid);
                        return true;
                    },
                });
            }
            else
            {
                string input = query.Search;
                if (input.Length != 32)
                {
                    throw new ArgumentException("Input UUID must be 32 characters long (without hyphens).");
                }

                string new_uuid = string.Format(CultureInfo.InvariantCulture, "{0}-{1}-{2}-{3}-{4}", input.Substring(0, 8), input.Substring(8, 4), input.Substring(12, 4), input.Substring(16, 4), input.Substring(20, 12));

                results.Add(new Result
                {
                    Title = new_uuid,
                    SubTitle = Description,
                    QueryTextDisplay = new_uuid,
                    IcoPath = _iconPath,
                    Action = action =>
                    {
                        Clipboard.SetText(new_uuid);
                        return true;
                    },
                });
            }

            return results;
        }

        public void Init(PluginInitContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _context.API.ThemeChanged += OnThemeChanged;
            UpdateIconPath(_context.API.GetCurrentTheme());
        }

        public string GetTranslatedPluginTitle()
        {
            return Resources.plugin_name;
        }

        public string GetTranslatedPluginDescription()
        {
            return Resources.plugin_description;
        }

        private void OnThemeChanged(Theme oldTheme, Theme newTheme)
        {
            UpdateIconPath(newTheme);
        }

        private void UpdateIconPath(Theme theme)
        {
            if (theme == Theme.Light || theme == Theme.HighContrastWhite)
            {
                _iconPath = "Images/UUIDConverter.light.png";
            }
            else
            {
                _iconPath = "Images/UUIDConverter.dark.png";
            }
        }

        public Control CreateSettingPanel()
        {
            throw new NotImplementedException();
        }

        public void ReloadData()
        {
            if (_context is null)
            {
                return;
            }

            UpdateIconPath(_context.API.GetCurrentTheme());
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                if (_context != null && _context.API != null)
                {
                    _context.API.ThemeChanged -= OnThemeChanged;
                }

                _disposed = true;
            }
        }
    }
}
