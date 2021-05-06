using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Web_Service.Models;
using Web_Service.Helpers;
using Client_WPF.Models;
using Client_WPF.Cache;
using Client_WPF.Helpers;

namespace Client_WPF.ViewModels
{
    public class ApplicationViewModel : INotifyPropertyChanged
    {
        public ApplicationViewModel()
        {
            db = new ParkingmachinesdbContext();
            dbSize = db.Parkingmachines.Count();

            Parkingmachines = new ObservableCollection<ParkingmachineInfo>();

            Cache = new Caching<ParkingmachineInfo>();
            CacheList = new ObservableCollection<ParkingmachineInfo>();

            Favorites = JDeserializer<ObservableCollection<ParkingmachineInfo>>.Deser(ReadWriter<ObservableCollection<ParkingmachineInfo>>.Read())
                ?? new ObservableCollection<ParkingmachineInfo>();
        }

        private readonly ParkingmachinesdbContext db;
        public int dbSize { get; private set; }
        public ObservableCollection<ParkingmachineInfo> Parkingmachines { get; set; }

        private ParkingmachineInfo selectedParkingmachine;
        public ParkingmachineInfo SelectedParkingmachine
        {
            get { return selectedParkingmachine; }
            set
            {
                if (value != null)
                {
                    if (value.Id != 0)
                    {
                        var item = Cache.GetOrCreate(value.Id.ToString(), value);

                        if (!CacheList.Contains(item))
                            CacheList.Insert(0, item);
                    }
                    else
                    {
                        CacheList.Remove(CacheList.FirstOrDefault(n => n.Id == 0));
                        CacheList.Insert(0, value);
                    }

                }

                selectedParkingmachine = value;
                OnPropertyChanged("");
            }
        }

        private Caching<ParkingmachineInfo> Cache { get; set; }
        public ObservableCollection<ParkingmachineInfo> CacheList { get; set; }

        public ObservableCollection<ParkingmachineInfo> Favorites { get; set; }

        // команда подгрузки данных в список
        private RelayCommand downCommand;
        public RelayCommand DownCommand
        {
            get
            {
                return downCommand ??
                  (downCommand = new RelayCommand(obj =>
                  {
                      int sizeList = (int)obj;

                      if (sizeList == 0)
                          sizeList = 1;

                      if (sizeList + 10 >= dbSize)
                      {
                          for (int i = sizeList; i < dbSize; i++)
                          {
                              Parkingmachines.Add(db.Parkingmachines.FirstOrDefault(p => p.Id == i));
                          }
                      }
                      else
                      {
                          for (int i = sizeList; i < sizeList + 10; i++)
                          {
                              Parkingmachines.Add(db.Parkingmachines.FirstOrDefault(p => p.Id == i));
                          }
                      }
                  },
                 (obj) => Parkingmachines.Count < dbSize));
            }
        }

        // команда добавления данных в избранное
        private RelayCommand addCommand;
        public RelayCommand AddCommand
        {
            get
            {
                return addCommand ??
                  (addCommand = new RelayCommand(obj =>
                  {
                      ParkingmachineInfo elem = obj as ParkingmachineInfo;

                      if (elem != null && elem.Id != 0)
                      {
                          ReadWriter<List<ParkingmachineInfo>>.Write(new List<ParkingmachineInfo>() { elem });
                      }
                      else if (elem != null)
                      {
                          System.Windows.MessageBox.Show("Выбранный вами элемент уже находится в избранном.",
                              "Предупреждение", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                      }

                  },
                  (obj) => CacheList.Count > 0));
            }
        }

        // команда удаления данных из кэша
        private RelayCommand removeCommand;
        public RelayCommand RemoveCommand
        {
            get
            {
                return removeCommand ??
                  (removeCommand = new RelayCommand(obj =>
                  {
                      ParkingmachineInfo elem = obj as ParkingmachineInfo;

                      if (elem != null && elem.Id != 0)
                      {
                          CacheList.Remove(elem);
                          Cache.Remove(elem.Id.ToString());
                      }
                      else if (elem != null)
                          CacheList.Remove(elem);

                  },
                 (obj) => CacheList.Count > 0));
            }
        }

        // команда обновления вкладки избранное
        private RelayCommand updateCommand;
        public RelayCommand UpdateCommand
        {
            get
            {
                return updateCommand ??
                  (updateCommand = new RelayCommand(obj =>
                  {
                      List<ParkingmachineInfo> temp_list = JDeserializer<List<ParkingmachineInfo>>.Deser(ReadWriter<List<ParkingmachineInfo>>.Read());

                      if (temp_list != null)
                      {
                          if (Favorites.Count == 0)
                          {
                              foreach (var item in temp_list)
                              {
                                  Favorites.Insert(0, item);
                              }
                          }
                          else
                          {
                              bool found;
                              foreach (ParkingmachineInfo itemT in temp_list)
                              {
                                  found = false;

                                  foreach (var itemD in Favorites)
                                  {
                                       if (itemD.NumberOfParkingMeter == itemT.NumberOfParkingMeter)
                                        found = true;
                                  }

                                  if (!found)
                                  {
                                      Favorites.Insert(0, itemT);
                                  }

                              }
                          }
                      }

                  }
                  ));
            }
        }

        // команда очистки избранного
        private RelayCommand clearCommand;
        public RelayCommand ClearCommand
        {
            get
            {
                return clearCommand ??
                  (clearCommand = new RelayCommand(obj =>
                  {
                      ReadWriter<object>.Write(null);
                      Favorites.Clear();

                  },
                  (obj) => Favorites.Count > 0));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}