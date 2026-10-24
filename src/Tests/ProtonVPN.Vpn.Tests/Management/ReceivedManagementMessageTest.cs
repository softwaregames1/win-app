/*
 * Copyright (c) 2026 Proton AG
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

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProtonVPN.Common.Core.Networking;
using ProtonVPN.Vpn.Management;

namespace ProtonVPN.Vpn.Tests.Management;

[TestClass]
public class ReceivedManagementMessageTest
{
    [TestMethod]
    public void Bandwidth_ShouldParseBytes()
    {
        ReceivedManagementMessage message = new(">BYTECOUNT:1024,2048");

        NetworkTraffic result = message.Bandwidth();

        result.BytesDownloaded.Should().Be(1024);
        result.BytesUploaded.Should().Be(2048);
    }

    [TestMethod]
    [DataRow(">BYTECOUNT")]
    [DataRow(">BYTECOUNT:")]
    [DataRow(">BYTECOUNT:1024")]
    [DataRow(">BYTECOUNT:abc,def")]
    [DataRow("")]
    public void Bandwidth_ShouldBeZero_WhenMessageIsMalformed(string messageText)
    {
        ReceivedManagementMessage message = new(messageText);

        NetworkTraffic result = message.Bandwidth();

        result.BytesDownloaded.Should().Be(0);
        result.BytesUploaded.Should().Be(0);
    }
}
