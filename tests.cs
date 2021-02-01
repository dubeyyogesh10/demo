  private List<GlyphInfo> glyphs;
        private GlyphInfo selectedGlyph;
        private string text = string.Empty;
      
        public ICollectionView GlyphsSource { get; set; }
        public DelegateCommand TextChangedCommand { get; set; }
        public DelegateCommand QuerySubmittedCommand { get; set; }

        public GlyphInfo SelectedGlyph
        {
            get { return selectedGlyph; }
            set 
            {
                selectedGlyph = value; 
                OnPropertyChanged("SelectedGlyph"); 
            }
        }

        public string Text
        {
            get { return text; }
            set 
            { 
                text = value;
                OnPropertyChanged("Text"); 
            }
        }

        public GlyphsViewModel()
        {
            this.glyphs = GlyphDataProvider.GetGlyphData();
            this.TextChangedCommand = new DelegateCommand(OnTextChangedCommandExecuted);
            this.QuerySubmittedCommand = new DelegateCommand(OnQuerySubmittedCommandExecuted);
            this.GlyphsSource = CollectionViewSource.GetDefaultView(this.glyphs);
            this.GlyphsSource.GroupDescriptions.Add(new PropertyGroupDescription("Category"));
            this.GlyphsSource.Filter = new Predicate<object>(FilterGlyphInfo);
        }

        private void OnQuerySubmittedCommandExecuted(object obj)
        {
            var querySumbittedArgs = (QuerySubmittedEventArgs)obj;
            if (querySumbittedArgs.Suggestion == null)
            {
                this.SelectedGlyph = this.glyphs.FirstOrDefault(x => FilterGlyphInfo(x));
                if (this.selectedGlyph != null)
                {
                    this.Text = this.selectedGlyph.Name;
                }                
            }
            else
            {
                this.SelectedGlyph = (GlyphInfo)querySumbittedArgs.Suggestion;
            }
        }

        private void OnTextChangedCommandExecuted(object obj)
        {
            var textChangedArgs = (TextChangedEventArgs)obj;
            if (textChangedArgs.Reason == TextChangeReason.UserInput)
            {
                this.GlyphsSource.Refresh();

                if (string.IsNullOrEmpty(this.text))
                {
                    this.SelectedGlyph = null;
                }
            }             
        }

        public bool FilterGlyphInfo(object value)
        {
            if (value != null)
            {
                return ((GlyphInfo)value).Name.ToLowerInvariant().Contains(this.text.ToLowerInvariant());
            }
            return false;           
        }
    }
