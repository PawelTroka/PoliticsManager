using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;
using Microsoft.Ajax.Utilities;
using PoliticsManager.Controllers;

namespace PoliticsManager.Models
{
    /* 132334 =>  4. partia

         * identyfikator (int)
         * nazwa (string)
         * liczba członków (int)
         * wynik w ostatnich wyborach (int)
         * lista partii, z którymi może wejść w koalicję (zbiór identyfikatorów)
     */
    public class PoliticalParty
    {
        public PoliticalParty()
        {
            FriendlyPoliticalPartiesIds = new List<int>();
        }

        // [Key]
        [Required]
        [Display(Description = "Identyfikator partii politycznej", Name = "Id")]
        public int Id { get; set; } //identyfikator (int)

        [Required]
        [MaxLength(25), MinLength(3)]
        [Display(Description = "Nazwa partii politycznej", Name = "Nazwa")]
        [RegularExpression("([A-Z][a-zA-Z ]+)", ErrorMessage = "Nazwa partii powinna się składać tylko z wyrazów i zaczynać się od dużej litery.")]
        public string Name { get; set; } //nazwa (string)

        [Required]
        [Range(1, 100000)]
        [Display(Description = "Liczba członków partii politycznej", Name = "Liczba członków")]
        public int MembersCount { get; set; } //liczba członków (int)


        [Required]
        [Range(0, 100)]
        [Display(Description = "Wynik w ostatnich wyborach partii politycznej", Name = "Wynik w ostatnich wyborach")]
        public int LastElectionResult { get; set; } //wynik w ostatnich wyborach (int)


        [Display(Description = "Lista partii politycznych, z którymi może wejść w koalicję", Name = "Lista partii, z którymi może wejść w koalicję")]
        public List<int> FriendlyPoliticalPartiesIds
        {
            get
            {
                if (m_FriendlyPoliticalPartiesIds == null) m_FriendlyPoliticalPartiesIds = new List<int>();
                return m_FriendlyPoliticalPartiesIds;
            }
            set { m_FriendlyPoliticalPartiesIds = value; }
        } //lista partii, z którymi może wejść w koalicję (zbiór identyfikatorów)


        private List<int> m_FriendlyPoliticalPartiesIds { get; set; } //lista partii, z którymi może wejść w koalicję (zbiór identyfikatorów)


        public List<SelectListItem> AllPoliticalPartiesIds
        {
            get
            {
                var db = new Database();
                var listOfIds = new List<int>();
                var selectList = new List<SelectListItem>();

                db.PoliticalParties.ForEach(pp =>
                {
                    if (pp.Id != this.Id)
                    {
                        listOfIds.Add(pp.Id);

                        var item = new SelectListItem();
                        item.Text = pp.Id.ToString();
                        item.Value = pp.Id.ToString();
                        item.Selected = (FriendlyPoliticalPartiesIds.Contains(pp.Id));
                        selectList.Add(item);
                    }
                });

                return selectList;
                //return listOfIds;
            }
        }

        public override string ToString()
        {
            return Id.ToString();
        }
    }


    public class Database
    {
        private HttpSessionState _session = HttpContext.Current.Session;
        private List<PoliticalParty> m_PoliticalParties
        {
            get
            {
                return _session["PoliticalParties"] as List<PoliticalParty>;
            }
            set { _session["PoliticalParties"] = value; }
        }

        public PoliticalParty Find(int id)
        {
            return m_PoliticalParties.Find(p => p.Id == id);
        }

        public void Edit(int id, PoliticalParty pp)
        {
            var party = Find(id);

            foreach (var propertyInfo in typeof(PoliticalParty).GetProperties())
            {
                if (propertyInfo.CanRead && propertyInfo.CanWrite)
                    propertyInfo.SetValue(party, propertyInfo.GetValue(pp));
            }
        }

        public List<PoliticalParty> PoliticalParties
        {
            get
            {
                if (m_PoliticalParties == null)
                    m_PoliticalParties = new List<PoliticalParty>();

                return m_PoliticalParties;
            }

            set { m_PoliticalParties = value; }
        }

        internal void SaveChanges()
        {
            return;//haha it doesnt do anything ;D
        }

        internal void Dispose()
        {
            return;//again nothing to see here ;)
        }
    }
}