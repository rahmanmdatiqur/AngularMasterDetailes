using Angular_MasterDetails.DTO;
using Angular_MasterDetails.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Angular_MasterDetails.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestEntriesController : ControllerBase
    {
        private readonly TestDbContext _context;
        private readonly IWebHostEnvironment _env;

        public TestEntriesController(TestDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            this._env = env;
        }

        [HttpGet]
        [Route("GetDiseses")]
        public async Task<ActionResult<IEnumerable<Disese>>> GetDiseses()
        {
            return await _context.Diseses.ToListAsync();
        }

        [HttpGet]
        [Route("GetPatients")]
        public async Task<ActionResult<IEnumerable<Patient>>> GetPatients()
        {
            return await _context.Patients.ToListAsync();
        }


        // GET: api/TestEntries
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TestDTO>>> GetTestEntries()
        {
            List<TestDTO> testDiseses = new List<TestDTO>();

            var allPatients = _context.Patients.ToList();
            foreach (var patient in allPatients)
            {
                var diseseList = _context.TestEntries.Where(x => x.PatientId == patient.PatientId).Select(x => new Disese { DiseseId = x.DiseseId }).ToList();
                testDiseses.Add(new TestDTO
                {
                    PatientId = patient.PatientId,
                    PatientName = patient.PatientName,
                    BirthDate = patient.BirthDate,
                    PhoneNo = patient.PhoneNo,
                    MaritalStatus = patient.MaritalStatus,
                    Picture = patient.Picture,
                    DiseseItems = diseseList.ToArray()
                });
            }


            return testDiseses;
        }

        // GET: api/TestEntries/id
        [HttpGet("{id}")]
        public async Task<ActionResult<TestDTO>> GetBookingEntries(int id)
        {
            Patient patient = await _context.Patients.FindAsync(id);
            Disese[] diseseList = _context.TestEntries.Where(x => x.PatientId == patient.PatientId).Select(x => new Disese { DiseseId = x.DiseseId }).ToArray();

            TestDTO testDisese = new TestDTO()
            {
                PatientId = patient.PatientId,
                PatientName = patient.PatientName,
                BirthDate = patient.BirthDate,
                PhoneNo = patient.PhoneNo,
                MaritalStatus = patient.MaritalStatus,
                Picture = patient.Picture,
                DiseseItems = diseseList.ToArray()
            };

            return testDisese;
        }


        // POST: api/TestEntries
        [HttpPost]
        public async Task<ActionResult<TestEntry>> PostTestEntry([FromForm] TestDTO testDTO)
        {

            var diseseItems = JsonConvert.DeserializeObject<Disese[]>(testDTO.diseseStringify);

            Patient patient = new Patient
            {
                PatientName = testDTO.PatientName,
                BirthDate = testDTO.BirthDate,
                PhoneNo = testDTO.PhoneNo,
                MaritalStatus = testDTO.MaritalStatus
            };

            if (testDTO.PictureFile != null)
            {
                var webroot = _env.WebRootPath;
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(testDTO.PictureFile.FileName);
                var filePath = Path.Combine(webroot, "Images", fileName);

                FileStream fileStream = new FileStream(filePath, FileMode.Create);
                await testDTO.PictureFile.CopyToAsync(fileStream);
                await fileStream.FlushAsync();
                fileStream.Close();
                patient.Picture = fileName;
            }

            foreach (var item in diseseItems)
            {
                var testEntry = new TestEntry
                {
                    Patient = patient,
                    PatientId = patient.PatientId,
                    DiseseId = item.DiseseId
                };
                _context.Add(testEntry);
            }

            await _context.SaveChangesAsync();

            return Ok(patient);
        }

        // POST: api/TestEntries
        [Route("Update")]
        [HttpPost]
        public async Task<ActionResult<TestEntry>> UpdateTestEntry([FromForm] TestDTO testDTO)
        {

            var diseseItems = JsonConvert.DeserializeObject<Disese[]>(testDTO.diseseStringify);

            Patient patient = await _context.Patients.FindAsync(testDTO.PatientId);

            patient.PatientName = testDTO.PatientName;
            patient.BirthDate = testDTO.BirthDate;
            patient.PhoneNo = testDTO.PhoneNo;
            patient.MaritalStatus = testDTO.MaritalStatus;


            if (testDTO.PictureFile != null)
            {
                var webroot = _env.WebRootPath;
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(testDTO.PictureFile.FileName);
                var filePath = Path.Combine(webroot, "Images", fileName);

                FileStream fileStream = new FileStream(filePath, FileMode.Create);
                await testDTO.PictureFile.CopyToAsync(fileStream);
                await fileStream.FlushAsync();
                fileStream.Close();
                patient.Picture = fileName;
            }

            //Delete existing Entries
            var existingEntries = _context.TestEntries.Where(x => x.PatientId == patient.PatientId).ToList();
            foreach (var item in existingEntries)
            {
                _context.TestEntries.Remove(item);
            }

            //Add newly added Entries
            foreach (var item in diseseItems)
            {
                var testEntries = new TestEntry
                {
                    PatientId = patient.PatientId,
                    DiseseId = item.DiseseId
                };
                _context.Add(testEntries);
            }

            _context.Entry(patient).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return Ok(patient);
        }

        //Delete Test Entry
        // POST: api/TestEntries/delete
        [Route("Delete/{id}")]
        [HttpPost]
        public async Task<ActionResult<TestEntry>> DeleteBookingEntry(int id)
        {

            Patient patient = _context.Patients.Find(id);

            var existingDiseses = _context.TestEntries.Where(x => x.PatientId == patient.PatientId).ToList();
            foreach (var item in existingDiseses)
            {
                _context.TestEntries.Remove(item);
            }

            _context.Entry(patient).State = EntityState.Deleted;

            await _context.SaveChangesAsync();

            return Ok(patient);
        }


    }
}
