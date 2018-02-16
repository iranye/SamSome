namespace BusinessLib
{
    public static class Database
    {
        public static Person GetFamilyTree()
        {
            // In a real app this method would access a database.
            return new Person
            {
                Name = "David Weatherbeam",
                Children =
                {
                    new Person
                    {
                        Name="Yvette Weatherbeam",
                        Children=
                        {
                            new Person
                            {
                                Name="Zena Hairmonger",
                                Children=
                                {
                                    new Person
                                    {
                                        Name="Sarah Applifunk",
                                        Children=
                                        {
                                            new Person
                                            {
                                                Name="Zena Hairmonger II",
                                                Children=
                                                {
                                                    new Person
                                                    {
                                                        Name="Sarah Applifunk II",
                                                        Children=
                                                        {
                                                            new Person
                                                            {
                                                                Name="Zena Hairmonger III",
                                                                Children=
                                                                {
                                                                    new Person
                                                                    {
                                                                        Name="Zena Hairmonger III",
                                                                        Children=
                                                                        {
                                                                            new Person
                                                                            {
                                                                                Name="Rocky Hairmonger",
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            },
                            new Person
                            {
                                Name="Verdi von NeuerBlast",
                                Children=
                                {
                                    new Person
                                    {
                                        Name = "Sunny Fundelica",
                                        Children=
                                        {
                                            new Person
                                            {
                                                Name = "Paco"
                                            },
                                            new Person
                                            {
                                                Name = "Ruiz"
                                            },
                                            new Person
                                            {
                                                Name = "Anna Luisa"
                                            }
                                        }
                                    },
                                    new Person
                                    {
                                        Name="Billy von Babblegait"
                                    },
                                    new Person
                                    {
                                        Name="Savah von Flipperwait"
                                    },
                                    new Person
                                    {
                                        Name="Nancy von Pippy"
                                    },
                                    new Person
                                    {
                                        Name="Angla von Rockermait"
                                    },
                                    new Person
                                    {
                                        Name="Rilko von Rikkytait"
                                    },
                                    new Person
                                    {
                                        Name="Funko von Funkyshades"
                                    }
                                }
                            },
                            new Person
                            {
                                Name="Jenny van Machoqueen",
                                Children=
                                {
                                    new Person
                                    {
                                        Name="Nick van Machoqueen",
                                    },
                                    new Person
                                    {
                                        Name="Matilda Porcupinicus",
                                    },
                                    new Person
                                    {
                                        Name="Bronco van Machoqueen",
                                    }
                                }
                            }
                        }
                    },
                    new Person
                    {
                        Name="Komrade Winkleford",
                        Children=
                        {
                            new Person
                            {
                                Name="Maurice Winkleford",
                                Children=
                                {
                                    new Person
                                    {
                                        Name="Divinity W. Llamafoot",
                                    }
                                }
                            },
                            new Person
                            {
                                Name="Komrade Winkleford, Jr.",
                                Children=
                                {
                                    new Person
                                    {
                                        Name="Saratoga Z. Crankentoe",
                                    },
                                    new Person
                                    {
                                        Name="Excaliber Winkleford",
                                        Children=
                                        {
                                            new Person
                                            {
                                                Name="Tyler Winkleford",
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };
        }
    }
}
