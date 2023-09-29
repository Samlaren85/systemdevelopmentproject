﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer;
using EntityLayer;


namespace BusinessLayer
{
    public class BookingController
    {
        public UnitOfWork unitOfWork;

        public void CreateBokning(Användare användarID, Kund kundID, List<Facilitet> facilitetsID, List<Utrustning> utrustningID, List<Aktivitet> AktivitetID)
        {
            Bokning bokning = new Bokning(användarID, kundID, facilitetsID, utrustningID, AktivitetID);
            unitOfWork.BokningsRepository.Add(bokning);
            unitOfWork.Save();
        }

    }
}