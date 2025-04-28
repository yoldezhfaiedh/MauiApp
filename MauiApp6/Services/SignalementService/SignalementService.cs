//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.Linq;
//using System.Threading.Tasks;
//using MauiApp6.Data;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Logging;

//public class SignalementService : ISignalementService
//{
//    private readonly RecruitingContext _context;
//    private readonly ILogger<SignalementService> _logger;

//    public SignalementService(RecruitingContext context, ILogger<SignalementService> logger)
//    {
//        _context = context ?? throw new ArgumentNullException(nameof(context));
//        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
//    }

//    public async Task<List<Signalements>> GetAllSignalementsAsync()
//    {
//        try
//        {
//            return await _context.Signalements
//                .AsNoTracking()
//                .OrderByDescending(s => s.DateSignalement)
//                .ToListAsync();
//        }
//        catch (Exception ex)
//        {
//            _logger.LogError(ex, "Erreur dans GetAllSignalementsAsync");
//            throw;
//        }
//    }

//    public async Task<Signalements> GetSignalementByIdAsync(int id)
//    {
//        try
//        {
//            return await _context.Signalements
//                .AsNoTracking()
//                .FirstOrDefaultAsync(s => s.Id == id);
//        }
//        catch (Exception ex)
//        {
//            _logger.LogError(ex, $"Erreur dans GetSignalementByIdAsync pour l'ID {id}");
//            throw;
//        }
//    }
//    public async Task<bool> AddSignalementAsync(Signalements s)
//    {
//        try
//        {
//            _logger.LogInformation("Tentative d'ajout : {@Signalement}", s);

//            // Vérification supplémentaire
//            if (s == null || string.IsNullOrEmpty(s.Titre)
//                return false;

//            _context.Signalements.Add(s);
//            var result = await _context.SaveChangesAsync();

//            _logger.LogInformation("Résultat : {Result} ligne affectée", result);
//            return result > 0;
//        }
//        catch (DbUpdateException dbEx)
//        {
//            _logger.LogError(dbEx, "Erreur DB : {Message}", dbEx.InnerException?.Message);
//            return false;
//        }
//        catch (Exception ex)
//        {
//            _logger.LogError(ex, "Erreur inattendue");
//            return false;
//        }
//    }
//    public async Task<bool> UpdateSignalementAsync(Signalements s)
//    {
//        if (s == null) throw new ArgumentNullException(nameof(s));

//        try
//        {
//            var existing = await _context.Signalements.FindAsync(s.Id);
//            if (existing == null) return false;

//            _context.Entry(existing).CurrentValues.SetValues(s);
//            await _context.SaveChangesAsync();
//            return true;
//        }
//        catch (Exception ex)
//        {
//            _logger.LogError(ex, $"Erreur dans UpdateSignalementAsync pour l'ID {s.Id}");
//            return false;
//        }
//    }

//    public async Task<bool> DeleteSignalementAsync(int id)
//    {
//        try
//        {
//            var s = await _context.Signalements.FindAsync(id);
//            if (s == null) return false;

//            _context.Signalements.Remove(s);
//            await _context.SaveChangesAsync();
//            return true;
//        }
//        catch (Exception ex)
//        {
//            _logger.LogError(ex, $"Erreur dans DeleteSignalementAsync pour l'ID {id}");
//            return false;
//        }
//    }

//    //public async Task<bool> TestConnectionAsync()
//    //{
//    //    try
//    //    {
//    //        return await _context.Database.CanConnectAsync();
//    //    }
//    //    catch (Exception ex)
//    //    {
//    //        _logger.LogError(ex, "Erreur dans TestConnectionAsync");
//    //        return false;
//    //    }
//    //}

//    public async Task<bool> TestConnectionAsync()
//    {
//        try
//        {
//            // Test 1: Vérification de la connexion basique
//            var canConnect = await _context.Database.CanConnectAsync();

//            if (!canConnect)
//            {
//                _logger.LogError("Échec de la connexion à la base de données");
//                return false;
//            }

//            // Test 2: Vérification que la table existe
//            var tableExists = await _context.Signalements.AnyAsync();
//            _logger.LogInformation("TestConnection: Table exists - {TableExists}", tableExists);

//            // Test 3: Exécution d'une requête simple
//            var testRecord = new Signalements
//            {
//                Titre = "Test Connection",
//                Description = "Enregistrement de test",
//                Email = "test@example.com",
//                DateSignalement = DateTime.Now,
//                Statut = "Test"
//            };

//            await _context.Signalements.AddAsync(testRecord);
//            var inserted = await _context.SaveChangesAsync() > 0;

//            if (inserted)
//            {
//                // Nettoyage
//                _context.Signalements.Remove(testRecord);
//                await _context.SaveChangesAsync();
//            }

//            return inserted;
//        }
//        catch (Exception ex)
//        {
//            _logger.LogError(ex, "Erreur lors du test de connexion à la base de données");
//            return false;
//        }
//    }
//}

//// Services/SignalementService.cs
//using System;
//using MauiApp6.Data;
//using Microsoft.EntityFrameworkCore;

//public class SignalementService : ISignalementService
//{
//    private readonly RecruitingContext _context;

//    public SignalementService(RecruitingContext context)
//    {
//        _context = context;
//    }

//    public async Task<bool> AjouterSignalementAsync(Signalement signalement)
//    {
//        try
//        {
//            await _context.Signalements.AddAsync(signalement);
//            await _context.SaveChangesAsync();
//            return true;
//        }
//        catch
//        {
//            return false;
//        }
//    }
//}