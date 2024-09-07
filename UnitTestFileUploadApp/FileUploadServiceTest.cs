namespace UnitTestFileUploadApp;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

[TestFixture]
    public class FileUploadServiceTest
    {
        private AppDbContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            return new AppDbContext(options);
        }

        private FileUploadService CreateService(AppDbContext context)
        {
            var logger = new Mock<ILogger<FileUploadService>>().Object;
            return new FileUploadService(context, logger);
        }

        [Test]
        public async Task SaveFileRecordAsync_ShouldSaveFileRecord()
        {
            // Arrange
            var context = CreateContext();
            var service = CreateService(context);

            var fileName = "testfile.txt";
            var filePath = "/path/to/file";
            var extension = ".txt";
            var createdOn = DateTime.UtcNow;
            var lastModifiedOn = DateTime.UtcNow;

            // Act
            await service.SaveFileRecordAsync(fileName, filePath, extension, createdOn, lastModifiedOn);

            // Assert
            var fileRecord = await context.FileRecords.FirstOrDefaultAsync(fr => fr.Name == fileName);
            Assert.NotNull(fileRecord);
            Assert.AreEqual(fileName, fileRecord.Name);
            Assert.AreEqual(filePath, fileRecord.Path);
            Assert.AreEqual(extension, fileRecord.Extension);
            Assert.AreEqual(createdOn, fileRecord.CreatedOn);
            Assert.AreEqual(lastModifiedOn, fileRecord.LastModifiedOn);
        }

        [Test]
        public async Task SaveFileRecordAsyncShouldThrowArgumentExceptionForInvalidInput()
        {
            // Arrange
            var context = CreateContext();
            var service = CreateService(context);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(() =>
                service.SaveFileRecordAsync("", "/path/to/file", ".txt", DateTime.UtcNow, DateTime.UtcNow));

            Assert.ThrowsAsync<ArgumentException>(() =>
                service.SaveFileRecordAsync("testfile.txt", "", ".txt", DateTime.UtcNow, DateTime.UtcNow));

            Assert.ThrowsAsync<ArgumentException>(() =>
                service.SaveFileRecordAsync("testfile.txt", "/path/to/file", "", DateTime.UtcNow, DateTime.UtcNow));
        }

        [Test]
        public async Task GetFileRecordAsync_ShouldReturnListOfFileRecords()
        {
            // Arrange
            var context = CreateContext();
            var service = CreateService(context);

            var fileRecord1 = new FileRecord
            {
                Name = "testfile1.txt",
                Path = "/path/to/file1",
                Extension = ".txt",
                CreatedOn = DateTime.UtcNow,
                LastModifiedOn = DateTime.UtcNow
            };
            var fileRecord2 = new FileRecord
            {
                Name = "testfile2.txt",
                Path = "/path/to/file2",
                Extension = ".txt",
                CreatedOn = DateTime.UtcNow,
                LastModifiedOn = DateTime.UtcNow
            };

            context.FileRecords.Add(fileRecord1);
            context.FileRecords.Add(fileRecord2);
            await context.SaveChangesAsync();

            // Act
            var result = await service.GetFileRecordAsync();

            // Assert
            Assert.AreEqual(2, result.Count);
        }

        [Test]
        public async Task GetFileRecordAsync_ShouldLogErrorOnException()
        {
            // Arrange
            var logger = new Mock<ILogger<FileUploadService>>();
            var context = CreateContext();
            var service = new FileUploadService(context, logger.Object);

            // Simulate an exception by setting the context to null (this is just an example)
            var faultyService = new FileUploadService(null, logger.Object);

            // Act
            var result = await faultyService.GetFileRecordAsync();

            // Assert
            logger.Verify(log => log.LogError(It.IsAny<Exception>(), It.IsAny<string>()), Times.AtLeastOnce);
        }
    }



