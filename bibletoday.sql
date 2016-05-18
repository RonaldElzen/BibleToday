-- phpMyAdmin SQL Dump
-- version 4.5.1
-- http://www.phpmyadmin.net
--
-- Host: 127.0.0.1
-- Gegenereerd op: 14 jan 2016 om 10:06
-- Serverversie: 10.1.9-MariaDB
-- PHP-versie: 5.6.15

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `bibletoday`
--

-- --------------------------------------------------------

--
-- Tabelstructuur voor tabel `planprogress`
--

CREATE TABLE `planprogress` (
  `planID` int(11) NOT NULL,
  `planname` varchar(255) NOT NULL,
  `lastVerse` varchar(255) NOT NULL,
  `dateStarted` datetime NOT NULL,
  `day` int(11) NOT NULL,
  `daysToGo` int(11) NOT NULL,
  `finished` varchar(8) NOT NULL DEFAULT 'No'
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Tabelstructuur voor tabel `planverses`
--

CREATE TABLE `planverses` (
  `planID` int(11) NOT NULL,
  `book` varchar(255) COLLATE utf8_unicode_ci NOT NULL,
  `chapter` int(11) NOT NULL,
  `startverse` int(11) NOT NULL,
  `endverse` int(11) NOT NULL,
  `day` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

--
-- Gegevens worden geëxporteerd voor tabel `planverses`
--

INSERT INTO `planverses` (`planID`, `book`, `chapter`, `startverse`, `endverse`, `day`) VALUES
(1, 'John', 1, 1, 2, 1),
(1, 'Luke', 2, 1, 2, 2),
(1, 'Isaiah', 41, 10, 11, 3);

-- --------------------------------------------------------

--
-- Tabelstructuur voor tabel `preferences`
--

CREATE TABLE `preferences` (
  `Name` varchar(255) CHARACTER SET latin1 NOT NULL,
  `Translation` varchar(255) CHARACTER SET latin1 NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf32 COLLATE=utf32_unicode_ci;

-- --------------------------------------------------------

--
-- Tabelstructuur voor tabel `readingplans`
--

CREATE TABLE `readingplans` (
  `planID` int(11) NOT NULL,
  `name` varchar(255) COLLATE utf8_unicode_ci NOT NULL,
  `description` varchar(255) COLLATE utf8_unicode_ci NOT NULL,
  `days` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

--
-- Gegevens worden geëxporteerd voor tabel `readingplans`
--

INSERT INTO `readingplans` (`planID`, `name`, `description`, `days`) VALUES
(1, 'Testplan', 'Testplan with 3 verses', 3);

-- --------------------------------------------------------

--
-- Tabelstructuur voor tabel `savedverses`
--

CREATE TABLE `savedverses` (
  `Book` varchar(255) NOT NULL,
  `Chapter` int(11) NOT NULL,
  `VerseFrom` int(11) NOT NULL,
  `VerseTo` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Indexen voor geëxporteerde tabellen
--

--
-- Indexen voor tabel `planprogress`
--
ALTER TABLE `planprogress`
  ADD KEY `planID` (`planID`);

--
-- Indexen voor tabel `planverses`
--
ALTER TABLE `planverses`
  ADD KEY `planID` (`planID`);

--
-- Indexen voor tabel `readingplans`
--
ALTER TABLE `readingplans`
  ADD PRIMARY KEY (`planID`);

--
-- AUTO_INCREMENT voor geëxporteerde tabellen
--

--
-- AUTO_INCREMENT voor een tabel `readingplans`
--
ALTER TABLE `readingplans`
  MODIFY `planID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;
--
-- Beperkingen voor geëxporteerde tabellen
--

--
-- Beperkingen voor tabel `planprogress`
--
ALTER TABLE `planprogress`
  ADD CONSTRAINT `planprogress_ibfk_1` FOREIGN KEY (`planID`) REFERENCES `readingplans` (`planID`);

--
-- Beperkingen voor tabel `planverses`
--
ALTER TABLE `planverses`
  ADD CONSTRAINT `planverses_ibfk_1` FOREIGN KEY (`planID`) REFERENCES `readingplans` (`planID`);

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
