using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XBBS.Models
{

    [PetaPoco.TableName("jexus_settings"),PetaPoco.PrimaryKey("sid")]
    public class Settings
    {
        [PetaPoco.Column(Name = "sid")]
        public int SId
        { get; set; }


        [PetaPoco.Column(Name = "title")]
        public string Title { get; set; }
        [PetaPoco.Column(Name = "value")]
        public string Value { get; set; }


        [PetaPoco.Column(Name = "type")]
        public int Type { get; set; }
    }

}
