﻿using System;
using System.Collections.Generic;

namespace DbUp.Engine;

/// <summary>
/// Represents the results of a database upgrade.
/// </summary>
public sealed class DatabaseUpgradeResult
{
    readonly List<SqlScript> scripts;

    /// <summary>
    /// Initializes a new instance of the <see cref="DatabaseUpgradeResult"/> class.
    /// </summary>
    /// <param name="scripts">The scripts that were executed.</param>
    /// <param name="successful">if set to <c>true</c> [successful].</param>
    /// <param name="error">The error.</param>
    /// <param name="errorScript">The script that was executing when the error occurred</param>
    public DatabaseUpgradeResult(IEnumerable<SqlScript> scripts, bool successful, Exception? error, SqlScript? errorScript)
    {
        this.scripts = new List<SqlScript>();
        this.scripts.AddRange(scripts);
        this.Successful = successful;
        this.Error = error;
        this.ErrorScript = errorScript;
    }

    /// <summary>
    /// Gets the scripts that were executed.
    /// </summary>
    public IEnumerable<SqlScript> Scripts => scripts;

    /// <summary>
    /// Gets a value indicating whether this <see cref="DatabaseUpgradeResult"/> is successful.
    /// </summary>
    public bool Successful { get; }

    /// <summary>
    /// Gets the error.
    /// </summary>
    public Exception? Error { get; }

    /// <summary>
    /// Gets the script that was executing when an error occurred.
    /// </summary>
    public SqlScript? ErrorScript { get; }
}
