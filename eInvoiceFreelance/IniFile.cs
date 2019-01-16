using System.IO;
using System.Text.RegularExpressions;
using System.Collections;
using System.Diagnostics;
using System;

// IniFile class used to read and write ini files by loading the file into memory
public class IniFile
{
	// List of IniSection objects keeps track of all the sections in the INI file
	private ArrayList _Sections;
	private string _FileName;

	// Gets all the sections names
	public System.Collections.ICollection SectionsName
	{
		get
		{
			return _Sections;
		}
	}

	// Public constructor
	public IniFile()
	{
		_Sections = new ArrayList();
	}

	// Loads the Reads the data in the ini file into the IniFile object
	public void Load(string file_name )
	{
		Load(file_name, false);
	}

	// Loads the Reads the data in the ini file into the IniFile object
	public void Load(string file_name, bool merge )
	{
		if (!merge)
		{
			RemoveAllSections();
			_FileName = "";
		}
		//  Clear the object...
		IniSection temp_section = null;
		_FileName = _FileName + "," + file_name;
		StreamReader stream_reader = new StreamReader(new FileStream(file_name, FileMode.OpenOrCreate, FileAccess.Read));
		Regex regex_comment = new Regex("^([\\s]*;.*)", (RegexOptions.Singleline | RegexOptions.IgnoreCase));
		Regex regex_section = new Regex("^[\\s]*\\[[\\s]*([^\\[\\s]*[^\\s\\]])[\\s]*\\][\\s]*$", (RegexOptions.Singleline | RegexOptions.IgnoreCase));
		Regex regex_key_value = new Regex("^\\s*([^=\\s]*)[^=]*=(.*)", (RegexOptions.Singleline | RegexOptions.IgnoreCase));
		Regex regex_key_value_comment = new Regex("^\\s*([^=\\s]*)[^=]*=(.*);(.*)", (RegexOptions.Singleline | RegexOptions.IgnoreCase));
		Regex regex_key_value_string = new Regex("^\\s*([^=\\s]*)[^=]*=\"(.*)\"", (RegexOptions.Singleline | RegexOptions.IgnoreCase));
		Regex regex_key_value_string_comment = new Regex("^\\s*([^=\\s]*)[^=]*=\"(.*)\";(.*)", (RegexOptions.Singleline | RegexOptions.IgnoreCase));
		while (!stream_reader.EndOfStream)
		{
			string line = stream_reader.ReadLine();
			if (line != string.Empty)
			{
				Match m = null;
				if (regex_comment.Match(line).Success)
				{
					// Comment
					m = regex_comment.Match(line);
					Trace.WriteLine(string.Format("Skipping Comment: {0}", m.Groups[0].Value));
				}
				else if (regex_section.Match(line).Success)
				{
					// Section
					m = regex_section.Match(line);
					Trace.WriteLine(string.Format("Adding section [{0}]", m.Groups[1].Value));
					temp_section = AddSection(m.Groups[1].Value);
				}
				else if (regex_key_value_string_comment.Match(line).Success && (temp_section != null))
				{
					// Key="Value";Comment
					m = regex_key_value_string_comment.Match(line);
					Trace.WriteLine(string.Format("Adding Key [{0}]=[{1}];[{2}]", m.Groups[1].Value, m.Groups[2].Value, m.Groups[3].Value));
					temp_section.AddKey(m.Groups[1].Value).Value = m.Groups[2].Value;
					temp_section.AddKey(m.Groups[1].Value).Comment = m.Groups[3].Value;
					temp_section.AddKey(m.Groups[1].Value).Quotes = true;
				}
				else if (regex_key_value_string.Match(line).Success && (temp_section != null))
				{
					// Key="Value"
					m = regex_key_value_string.Match(line);
					Trace.WriteLine(string.Format("Adding Key [{0}]=\"[{1}]\"", m.Groups[1].Value, m.Groups[2].Value));
					temp_section.AddKey(m.Groups[1].Value).Value = m.Groups[2].Value;
					temp_section.AddKey(m.Groups[1].Value).Quotes = true;
				}
				else if (regex_key_value_comment.Match(line).Success && (temp_section != null))
				{
					// Key=Value;Comment
					m = regex_key_value_comment.Match(line);
					Trace.WriteLine(string.Format("Adding Key [{0}]=[{1}];[{2}]", m.Groups[1].Value, m.Groups[2].Value, m.Groups[3].Value));
					temp_section.AddKey(m.Groups[1].Value).Value = m.Groups[2].Value;
					temp_section.AddKey(m.Groups[1].Value).Comment = m.Groups[3].Value;
					temp_section.AddKey(m.Groups[1].Value).Quotes = false;
				}
				else if (regex_key_value.Match(line).Success && (temp_section != null))
				{
					// Key=Value
					m = regex_key_value.Match(line);
					Trace.WriteLine(string.Format("Adding Key [{0}]=[{1}]", m.Groups[1].Value, m.Groups[2].Value));
					temp_section.AddKey(m.Groups[1].Value).Value = m.Groups[2].Value;
					temp_section.AddKey(m.Groups[1].Value).Quotes = false;
				}
				else if (temp_section != null)
				{
					//  Handle Key without value
					Trace.WriteLine(string.Format("Adding Key [{0}]", line));
					temp_section.AddKey(line);
				}
				else
				{
					//  This should not occur unless the tempsection is not created yet...
					Trace.WriteLine(string.Format("Skipping unknown type of data: {0}", line));
				}
			}
		}

		stream_reader.Close();
	}

	// Used to save the data back to the file or your choice
	public void Save(string file_name)
	{
		StreamWriter stream_writer = new StreamWriter(file_name, false);
		foreach (IniSection s in SectionsName)
		{
			Trace.WriteLine(string.Format("Writing Section: [{0}]", s.Name));
			stream_writer.WriteLine(string.Format("[{0}]", s.Name));
			foreach (IniSection.IniKey k in s.Keys)
			{
				if (k.Comment == null)
				{
					if (k.Quotes == true)
					{
						Trace.WriteLine(string.Format("Writing Key: {0}={1}", k.Name, k.Value));
						stream_writer.WriteLine(string.Format("{0}=\"{1}\"", k.Name, k.Value));
					}
					else
					{
						Trace.WriteLine(string.Format("Writing Key: {0}={1}", k.Name, k.Value));
						stream_writer.WriteLine(string.Format("{0}={1}", k.Name, k.Value));
					}

				}
				else
				{
					if (k.Quotes == true)
					{
						Trace.WriteLine(string.Format("Writing Key: {0}=\"{1}\";{2}", k.Name, k.Value, k.Comment));
						stream_writer.WriteLine(string.Format("{0}=\"{1}\";{2}", k.Name, k.Value, k.Comment));
					}
					else
					{
						Trace.WriteLine(string.Format("Writing Key: {0}={1};{2}", k.Name, k.Value, k.Comment));
						stream_writer.WriteLine(string.Format("{0}={1};{2}", k.Name, k.Value, k.Comment));
					}
				}
			}
		}
		stream_writer.Close();
	}

	// Adds a section to the IniFile object, returns a IniSection object to the new or existing object
	public IniSection AddSection(string section_name )
	{
		int id;

		IniSection s = null;
		section_name = section_name.Trim();

		id = _Sections.IndexOf(section_name.Trim());
		if (id == -1)
		{
			s = new IniSection(this, section_name);
			_Sections.Add(s);
		}
		else
		{
			s = (IniSection)_Sections[id];
		}

		return (s);
	}

	// Removes a section by its name section_name, returns trus on success
	public bool RemoveSection(string section_name)
	{
		section_name = section_name.Trim();
		return RemoveSection(GetSection(section_name));
	}

	// Removes section by object, returns trus on success
	public bool RemoveSection(IniSection section)
	{
		if (section != null)
		{
			try
			{
				_Sections.Remove(section.Name);
				return true;
			}
			catch ( Exception ex )
			{
				Trace.WriteLine(ex.Message);
			}
		}
		return false;
	}

	//  Removes all existing sections, returns trus on success
	public bool RemoveAllSections()
	{
		_Sections.Clear();
		return (_Sections.Count == 0);
	}

	// Returns an IniSection to the section by name, NULL if it was not found
	public IniSection GetSection(string section_name)
	{
		int id;

		id = _Sections.IndexOf(section_name.Trim());
		if (id == -1)
		{
			return (null);
		}
		else
		{
			return ((IniSection)_Sections[id]);
		}

	}

	//  Returns a KeyValue in a certain section
	public string GetKeyValue(string section_name, string key_name)
	{
		IniSection s;
		IniSection.IniKey k;

		s = GetSection(section_name);
		if (s == null)
		{
			s = AddSection(section_name);
			k = s.AddKey(key_name);
		}
		else
		{
			k = s.GetKey(key_name);
			if (k == null)
			{
				k = s.AddKey(key_name);
			}
			else
			{
				if (k.Value == null)
				{
					k.Value = "";
				}

				return (k.Value);
			}
		}

		return (string.Empty);
	}

	// Sets a KeyValuePair in a certain section
	public bool SetKeyValue(string section_name, string key_name, string value)
	{
		IniSection s;
		IniSection.IniKey k;

		s = AddSection(section_name);
		if (s != null)
		{
			k = s.AddKey(key_name);
			if (k != null)
			{
				k.Value = value;
				return (true);
			}
		}
		return (false);
	}

	// Renames an existing section returns true on success, false if the section didn't exist or there was another section with the same sNewSection
	public bool RenameSection(string old_section_name, string new_section_name)
	{
		//  Note string trims are done in lower calls.
		IniSection s = GetSection(old_section_name);
		if (s != null)
		{
			return (s.SetName(new_section_name));

		}
		return (false);
	}

	// Renames an existing key returns true on success, false if the key didn't exist or there was another section with the same sNewKey
	public bool RenameKey(string section_name, string old_key_name, string new_key_name)
	{
		//  Note string trims are done in lower calls.
		IniSection s = GetSection(section_name);
		if (s != null)
		{
			IniSection.IniKey k = s.GetKey(old_key_name);
			if (k != null)
			{
				return (k.SetName(new_key_name));
			}
		}
		return (false);
	}

	public string FileName
	{
		get
		{
			return (_FileName);
		}
	}

	// IniSection class
	public class IniSection
	{
		//  IniFile IniFile object instance
		private IniFile _IniFile;
		//  Name of the section
		private string _SectionName;
		//  List of IniKeys in the section
		private ArrayList _Keys;

		// Constuctor so objects are internally managed
		protected internal IniSection(IniFile parent, string section_name)
		{
			_IniFile = parent;
			_SectionName = section_name;
			_Keys = new ArrayList();
		}

		// Override Equals function
		public override bool Equals(Object o)
		{
			return (string)o == _SectionName;
		}

		// Override GetHashCode
		public override int GetHashCode()
		{
			return (_SectionName.GetHashCode());
		}

		// Returns and hashtable of keys associated with the section
		public System.Collections.ICollection Keys
		{
			get
			{
				return _Keys;
			}
		}

		// Returns the section name
		public string Name
		{
			get
			{
				return _SectionName;
			}
		}

		// Adds a key to the IniSection object, returns a IniKey object to the new or existing object
		public IniKey AddKey(string key_name)
		{
			int id;

			key_name = key_name.Trim();
			IniSection.IniKey k = null;
			if (key_name.Length != 0)
			{
				id = _Keys.IndexOf(key_name);
				if (id == -1)
				{
					k = new IniSection.IniKey(this, key_name);
					_Keys.Add(k);
				}
				else
				{
					k = (IniKey)_Keys[id];
				}
			}
			return k;
		}

		// Removes a single key by string
		public bool RemoveKey(string key_name)
		{
			return RemoveKey(GetKey(key_name));
		}

		// Removes a single key by IniKey object
		public bool RemoveKey(IniKey key)
		{
			if (key != null)
			{
				try
				{
					_Keys.Remove(key.Name);
					return true;
				}
				catch (Exception ex)
				{
					Trace.WriteLine(ex.Message);
				}
			}
			return false;
		}

		// Removes all the keys in the section
		public bool RemoveAllKeys()
		{
			_Keys.Clear();
			return (_Keys.Count == 0);
		}

		// Returns a IniKey object to the key by name, NULL if it was not found
		public IniKey GetKey(string key_name)
		{
			int id;

			id = _Keys.IndexOf(key_name);
			if (id == -1)
			{
				return (null);
			}
			else
			{
				return ((IniKey)_Keys[id]);
			}
		}

		// Sets the section name, returns true on success, fails if the section
		// name section_name already exists
		public bool SetName(string section_name)
		{
			section_name = section_name.Trim();
			if (section_name.Length != 0)
			{
				// Get existing section if it even exists...
				IniSection s = _IniFile.GetSection(section_name);
				if (s != this && s != null)
				{
					return false;
				}
				try
				{
					// Remove the current section
					_IniFile._Sections.Remove(_SectionName);
					// Set the new section name to this object
					_IniFile._Sections.Add(this);
					// Set the new section name
					_SectionName = section_name;
					return (true);
				}
				catch (Exception ex)
				{
					Trace.WriteLine(ex.Message);
				}
			}
			return (false);
		}

		// Returns the section name
		public string GetName()
		{
			return _SectionName;
		}

		// IniKey class
		public class IniKey
		{
			//  Name of the Key
			private string _KeyName;
			//  Value associated
			private string _Value;
			//  Comment associated
			private string _Comment;
			//  Has "" to enclose the value
			private bool _Quotes;
			//  Pointer to the parent CIniSection
			private IniSection _Section;

			// Constuctor so objects are internally managed
			protected internal IniKey(IniSection parent, string key_name)
			{
				_Section = parent;
				_KeyName = key_name;
			}

			// Override Equals function
			public override bool Equals(Object o)
			{
				return (string)o == _KeyName;
			}

			// Override GetHashCode
			public override int GetHashCode()
			{
				return (_KeyName.GetHashCode());
			}

			// Returns the name of the Key
			public string Name
			{
				get
				{
					return _KeyName;
				}
			}

			// Sets or Gets the value of the key
			public string Value
			{
				get
				{
					return _Value;
				}
				set
				{
					_Value = value;
				}
			}

			// Sets or Gets the comment of the key
			public string Comment
			{
				get
				{
					return _Comment;
				}
				set
				{
					_Comment = value;
				}
			}

			// Sets or Gets the comment of the key
			public bool Quotes
			{
				get
				{
					return _Quotes;
				}
				set
				{
					_Quotes = value;
				}
			}

			// Sets the value of the key
			public void SetValue(string value)
			{
				_Value = value;
			}
			// Returns the value of the Key
			public string GetValue()
			{
				return _Value;
			}

			// Sets the value of the key
			public void SetComment(string comment)
			{
				_Comment = comment;
			}
			// Returns the value of the Key
			public string GetComment()
			{
				return _Comment;
			}

			// Sets the key name
			// Returns true on success, fails if the section name key_name already exists
			public bool SetName(string key_name)
			{
				key_name = key_name.Trim();
				if (key_name.Length != 0)
				{
					IniKey k = _Section.GetKey(key_name);
					if (k != this && k != null)
					{
						return false;
					}
					try
					{
						// Remove the current key
						_Section._Keys.Remove(_KeyName);
						// Set the new key name to this object
						_Section._Keys.Add(this);
						// Set the new key name
						_KeyName = key_name;

						return true;
					}
					catch (Exception ex)
					{
						Trace.WriteLine(ex.Message);
					}
				}
				return false;
			}

			// Returns the name of the Key
			public string GetName()
			{
				return _KeyName;
			}
		} // End of IniKey class
	} // End of IniSection class
} // End of IniFile class

