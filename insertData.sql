-- Inserts for Media (15 records: 5 Books, 5 DVDs, 3 CDs, 2 LPs)
INSERT INTO Media (title, price, totalQuantity, weight, rushOrderSupported, imageUrl, barcode, description, productDimension, importDate)
VALUES 
('The Great Gatsby', 150000, 100, 0.5, 1, 'Media1', '9780743273565', 'A classic novel by F. Scott Fitzgerald.', '20x13x2 cm', '2023-01-01'),
('To Kill a Mockingbird', 160000, 80, 0.6, 1, 'Media2', '9780061120084', 'A novel about racial injustice.', '21x14x2 cm', '2023-01-02'),
('1984', 140000, 90, 0.45, 0, 'Media3', '9780451524935', 'A dystopian novel by George Orwell.', '19x12x2 cm', '2023-01-03'),
('Pride and Prejudice', 130000, 70, 0.4, 1, 'Media4', '9780141439518', 'A romantic novel by Jane Austen.', '20x13x2 cm', '2023-01-04'),
('Moby Dick', 180000, 50, 0.8, 0, 'Media5', '9781503280786', 'An epic tale of a whale.', '22x15x3 cm', '2023-01-05'),

('The Matrix', 200000, 60, 0.3, 1, 'Media6', 'DVD0001', 'Sci-fi action movie.', '19x13x1.5 cm', '2023-02-01'),
('Inception', 210000, 55, 0.35, 1, 'Media7', 'DVD0002', 'Mind-bending thriller.', '19x13x1.5 cm', '2023-02-02'),
('The Dark Knight', 220000, 50, 0.4, 1, 'Media8', 'DVD0003', 'Batman sequel.', '19x13x1.5 cm', '2023-02-03'),
('The Lord of the Rings', 230000, 45, 0.45, 0, 'Media9', 'DVD0004', 'Fantasy adventure.', '19x13x1.5 cm', '2023-02-04'),
('Star Wars V', 240000, 65, 0.5, 1, 'Media10', 'DVD0005', 'Intergalactic epic.', '19x13x1.5 cm', '2023-02-05'),

('Abbey Road', 170000, 40, 0.25, 1, 'Media11', 'LP0001', 'The Beatles classic album.', '31x31x0.5 cm', '2023-03-01'),
('The Dark Side of the Moon', 175000, 35, 0.27, 1, 'Media12', 'LP0002', 'Pink Floyd masterpiece.', '31x31x0.5 cm', '2023-03-02'),

('Hotel California', 165000, 30, 0.2, 1, 'Media13', 'CD0001', 'Eagles top hits.', '14x12x1 cm', '2023-03-03'),
('Back in Black', 160000, 25, 0.22, 0, 'Media14', 'CD0002', 'AC/DC classic album.', '14x12x1 cm', '2023-03-04'),
('Thriller', 185000, 20, 0.23, 1, 'Media15', 'CD0003', 'Michael Jackson best-seller.', '14x12x1 cm', '2023-03-05');

-- Inserts for Book (media_id 1–5)
INSERT INTO Book (media_id, authors, coverType, publisher, publicationDate, pages, language, genre)
VALUES 
(1, 'F. Scott Fitzgerald', 'Hardcover', 'Scribner', '1925-04-10', 180, 'English', 'Classic'),
(2, 'Harper Lee', 'Hardcover', 'J. B. Lippincott & Co.', '1960-07-11', 281, 'English', 'Fiction'),
(3, 'George Orwell', 'Paperback', 'Secker & Warburg', '1949-06-08', 328, 'English', 'Dystopian'),
(4, 'Jane Austen', 'Hardcover', 'T. Egerton', '1813-01-28', 279, 'English', 'Romance'),
(5, 'Herman Melville', 'Paperback', 'Harper & Brothers', '1851-11-14', 635, 'English', 'Adventure');

-- Inserts for DVD (media_id 6–10)
INSERT INTO DVD (media_id, dvdType, director, runtime, studio, language, subtitles, releasedDate, genre)
VALUES 
(6, 'Blu-ray', 'Wachowski Sisters', 136, 'Warner Bros.', 'English', 'English, Spanish', '1999-03-31', 'Science Fiction'),
(7, 'Blu-ray', 'Christopher Nolan', 148, 'Warner Bros.', 'English', 'English, French', '2010-07-16', 'Thriller'),
(8, 'DVD', 'Christopher Nolan', 152, 'Warner Bros.', 'English', 'English, German', '2008-07-18', 'Action'),
(9, 'DVD', 'Peter Jackson', 201, 'New Line Cinema', 'English', 'English, Elvish', '2001-12-19', 'Fantasy'),
(10, 'DVD', 'George Lucas', 121, 'Lucasfilm', 'English', 'English, Spanish', '1977-05-25', 'Science Fiction');

-- Inserts for CD_and_LP (media_id 11–15)
INSERT INTO CD_and_LP (media_id, isCD, artists, recordLabel, trackList, genre, releaseDate)
VALUES 
(11, 0, 'The Beatles', 'Apple Records', 'Come Together, Something, Oh! Darling', 'Rock', '1969-09-26'),
(12, 0, 'Pink Floyd', 'Harvest Records', 'Speak to Me, Time, Money', 'Progressive Rock', '1973-03-01'),
(13, 1, 'Eagles', 'Asylum Records', 'Hotel California, New Kid in Town, Life in the Fast Lane', 'Rock', '1976-12-08'),
(14, 1, 'AC/DC', 'Atlantic Records', 'Hells Bells, Shoot to Thrill, Back in Black', 'Hard Rock', '1980-07-25'),
(15, 1, 'Michael Jackson', 'Epic Records', 'Thriller, Beat It, Billie Jean', 'Pop', '1982-11-30');
