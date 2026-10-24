/*
 * Copyright (c) 2023 Proton AG
 *
 * This file is part of ProtonVPN.
 *
 * ProtonVPN is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * ProtonVPN is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with ProtonVPN.  If not, see <https://www.gnu.org/licenses/>.
 */

using System.Collections.Generic;

namespace ProtonVPN.Api.Handlers.TlsPinning
{
    public class CachingReportClient : IReportClient
    {
        private readonly HashSet<string> _cache = new();
        private readonly object _lock = new();

        private readonly IReportClient _origin;

        public CachingReportClient(IReportClient origin)
        {
            _origin = origin;
        }

        public void Send(ReportBody body)
        {
            string hash = body.Hash();

            // Certificate validation callbacks can run concurrently on multiple threads
            lock (_lock)
            {
                if (!_cache.Add(hash))
                {
                    return;
                }
            }

            _origin.Send(body);
        }
    }
}